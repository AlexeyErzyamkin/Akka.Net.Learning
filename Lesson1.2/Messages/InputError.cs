using System;

namespace Lesson.Messages
{
    class InputError
    {
        public InputError(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; }
    }

    class NullInputError : InputError
    {
        public NullInputError(string reason) : base(reason)
        {
        }
    }

    class ValidationError : InputError
    {
        public ValidationError(string reason) : base(reason)
        {
        }
    }
}
