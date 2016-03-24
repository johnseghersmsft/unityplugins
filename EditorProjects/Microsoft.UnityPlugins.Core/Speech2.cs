using System;
using System.Collections.Generic;

namespace Microsoft.UnityPlugins
{
    public sealed class Speech2
    {
        public void CompileSgrsConstraints(IEnumerable<string> constraintFiles, Action<bool> onComplete)
        {
            if (onComplete != null)
                onComplete(false);
        }

        public void EnableConstraint(string filename, bool enable)
        {
        }

        public void ListenForConstrainedText(Action<CallbackResponse<SpeechArguments>> onSpeechResults)
        {
            if (onSpeechResults != null)
            {
                onSpeechResults(new CallbackResponse<SpeechArguments> { Result = null, Exception = new Exception("Cannot call Windows Store API in the Unity Editor"), Status = CallbackStatus.Failure });
            }

        }

        public void Stop()
        {
        }
       

    }

}
