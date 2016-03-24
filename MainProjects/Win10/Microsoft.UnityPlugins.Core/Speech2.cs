using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;
using Windows.Storage;

namespace Microsoft.UnityPlugins
{
    public sealed class Speech2
    {
        public SpeechRecognizer Recognizer
        {
            get { return _speechRecognizer ?? (_speechRecognizer = new SpeechRecognizer()); }
        }

        private SpeechRecognizer _speechRecognizer = null;
        private bool _isListening = false;
        private bool _eventsRegistered = false;
        private Action<CallbackResponse<SpeechArguments>> _onSpeechResults;
        private async Task ResetRecognizer()
        {
            await StopFromWindowsUiThreadContext();
            Recognizer.Constraints.Clear();
        }
        public void CompileSgrsConstraints(IEnumerable<string> constraintFiles, Action<bool> onComplete)
        {
            Utils.RunOnWindowsUIThread(async () =>
            {
                try
                {
                    await ResetRecognizer();
                    foreach (var filename in constraintFiles)
                    {
                        DebugLog.Log(LogLevel.Info, "Compiling Constraint " + filename);
                        var file = await StorageFile.GetFileFromPathAsync(filename);
                        var constraint = new SpeechRecognitionGrammarFileConstraint(file, filename);
                        Recognizer.Constraints.Add(constraint);
                    }
                    var compilationResult = await Recognizer.CompileConstraintsAsync();
                    if (compilationResult.Status != SpeechRecognitionResultStatus.Success)
                    {
                        DebugLog.Log(LogLevel.Error, "Constraint Compilation failed: " + compilationResult.Status);
                        onComplete(false);
                        return;
                    }
                    foreach (var constraint in Recognizer.Constraints)
                    {
                        constraint.IsEnabled = false;
                    }
                    onComplete(true);
                }
                catch (Exception e)
                {
                    DebugLog.Log(LogLevel.Error, "Unable to open SGRS file " + e.Message);
                    onComplete(false);
                }
            });
        }

        public void EnableConstraint(string filename, bool enable)
        {
            foreach (var constraint in Recognizer.Constraints)
            {
                if (constraint.Tag == filename)
                {
                    constraint.IsEnabled = enable;
                    return;
                }
            }
        }



        public void ListenForConstrainedText(Action<CallbackResponse<SpeechArguments>> onSpeechResults)
        {
            Utils.RunOnWindowsUIThread(async () =>
            {
                try
                {
                    await StopFromWindowsUiThreadContext();
                    DebugLog.Log(LogLevel.Info, "ListenForConstrainedText");
                    foreach (var constraint in Recognizer.Constraints)
                    {
                        if (constraint.IsEnabled)
                            DebugLog.Log(LogLevel.Info, "...Constraint: " + constraint.Tag);
                    }

                    _onSpeechResults = onSpeechResults;

                    if (!_eventsRegistered)
                    {
                        Recognizer.HypothesisGenerated += OnHypothesisGenerated;
                        Recognizer.ContinuousRecognitionSession.Completed += OnSessionCompleted;
                        Recognizer.ContinuousRecognitionSession.ResultGenerated += OnResultGenerated;
                        _eventsRegistered = true;
                    }
                    
                    await Recognizer.ContinuousRecognitionSession.StartAsync();
                    _isListening = true;
                }
                catch (Exception ex)
                {
                    onSpeechResults(new CallbackResponse<SpeechArguments>
                    {
                        Result = null,
                        Status = CallbackStatus.Failure,
                        Exception = ex
                    });
                    return;
                }
            });

        }


        private void OnHypothesisGenerated(SpeechRecognizer sender, SpeechRecognitionHypothesisGeneratedEventArgs args)
        {
            string hypothesis = args.Hypothesis.Text;
            DebugLog.Log(LogLevel.Info, "ListenForConstrainedText Hypothesis " + hypothesis + "...");
            var localCaptureOnSpeechResults = _onSpeechResults;
            if (localCaptureOnSpeechResults != null)
            {
                Utils.RunOnUnityAppThread(
                    () =>
                    {
                        localCaptureOnSpeechResults(new CallbackResponse<SpeechArguments>
                        {
                            Result =
                                new SpeechArguments
                                {
                                    Status = SpeechResultStatus.Hypothesis,
                                    Text = hypothesis
                                },
                            Status = CallbackStatus.Success,
                            Exception = null
                        });
                    });
            }
        }

        private void OnSessionCompleted(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
        {
            DebugLog.Log(LogLevel.Info, "ListenForConstrainedText " + args.Status.ToString());
            if (args.Status != SpeechRecognitionResultStatus.Success)
            {
                var localCaptureOnSpeechResults = _onSpeechResults;
                if (localCaptureOnSpeechResults != null)
                {
                    Utils.RunOnUnityAppThread(
                        () =>
                        {
                            localCaptureOnSpeechResults(new CallbackResponse<SpeechArguments>
                            {
                                Result = new SpeechArguments { Status = SpeechResultStatus.Complete },
                                Status = CallbackStatus.Success,
                                Exception = null
                            });
                        });
                }
                _isListening = false;
            }
        }
        private void OnResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            DebugLog.Log(LogLevel.Info, args.Result.Text);

            DebugLog.Log(LogLevel.Info, "ListenForConstrainedText " + args.Result.Text);
            var localCaptureOnSpeechResults = _onSpeechResults;
            if (localCaptureOnSpeechResults != null)
            {
                Utils.RunOnUnityAppThread(
                    () =>
                    {
                        localCaptureOnSpeechResults(new CallbackResponse<SpeechArguments>
                        {
                            Result =
                                new SpeechArguments
                                {
                                    Status = SpeechResultStatus.Dictation,
                                    Text = args.Result.Text,
                                    Confidence = (SpeechResultConfidence) (int) args.Result.Confidence
                                },
                            Status = CallbackStatus.Success,
                            Exception = null
                        });
                    });
            }
        }


        private async Task StopFromWindowsUiThreadContext()
        {
            _onSpeechResults = null;
            if (Recognizer != null)
            {
                try
                {
                    if (_isListening)
                    {
                        await Recognizer.ContinuousRecognitionSession.StopAsync();
                        _isListening = false;
                    }
                }
                catch (Exception ex)
                {
                    DebugLog.Log(LogLevel.Error, "Exception on Stop!" + ex);
                    _isListening = false;
                }
            }
        }
        public void Stop()
        {
            Utils.RunOnWindowsUIThread(async () => await StopFromWindowsUiThreadContext());
        }
       

    }

}
