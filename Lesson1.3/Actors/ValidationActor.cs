using System;
using Akka.Actor;

namespace Lesson.Actors
{
    class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            if (!(message is string read)) return;

            if (string.IsNullOrWhiteSpace(read))
            {
                _consoleWriterActor.Tell(new Messages.NullInputError("No input received."));
            }
            else
            {
                var valid = IsValid(read);
                if (valid)
                {
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you!"));
                }
                else
                {
                    _consoleWriterActor.Tell(new Messages.ValidationError("Inalid: Input had odd number of chars."));
                }
            }

            Sender.Tell(new Messages.ContinueProcessing());

            bool IsValid(string checking) => checking.Length % 2 == 0;
        }
    }
}
