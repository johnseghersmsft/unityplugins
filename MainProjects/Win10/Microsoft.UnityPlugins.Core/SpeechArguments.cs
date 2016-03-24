using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.UnityPlugins
{
    public enum SpeechResultStatus
    {
        Command = 0, 
        Complete = 1,
        Dictation = 2,
        Hypothesis = 3
    }

    public enum SpeechResultConfidence
    {
        High = 0,
        Medium = 1,
        Low = 2,
        Rejected = 3
    }
    public class SpeechArguments
    {
        public SpeechResultStatus Status { get; set; }

        public string Text { get; set; }

        public SpeechResultConfidence Confidence { get; set; }

    }


}