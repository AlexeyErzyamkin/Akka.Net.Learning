using System;
using Akka.Actor;

namespace Lesson.Actors
{
    class FileValidatorActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public FileValidatorActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var read = message as string;
            if (string.IsNullOrEmpty(read))
            {
                _consoleWriterActor.Tell(new Messages.NullInputError("No input received."));

                Sender.Tell(new Messages.ContinueProcessing());
            }
            else
            {
                var valid = IsFileUri(read);
                if (valid)
                {
                    _consoleWriterActor.Tell(new Messages.InputSuccess($"Starting processing for {read}"));
                    Context.ActorSelection("akka://Lesson/user/tail_coordinator").Tell(new TailCoordinatorActor.StartTail(read, _consoleWriterActor));
                }
                else
                {
                    _consoleWriterActor.Tell(new Messages.ValidationError("Inalid: Input had odd number of chars."));

                    Sender.Tell(new Messages.ContinueProcessing());
                }
            }

            bool IsFileUri(string path) => System.IO.File.Exists(path);
        }
    }
}
