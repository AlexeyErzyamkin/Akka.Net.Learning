using System;

namespace Lesson.Messages
{
    class InputSuccess
    {
        public InputSuccess(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; }
    }
}
