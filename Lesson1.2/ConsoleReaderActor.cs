using System;
using Akka.Actor;

namespace Lesson
{
    class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        public const string StartCommand = "start";

        private readonly IActorRef _consoleWriterActor;

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case StartCommand:
                {
                    PrintInstructions();
                    break;
                }

                case Messages.InputError error:
                {
                    _consoleWriterActor.Tell(error);
                    break;
                }
            }
            
            ReadAndValidateInput();
        }

        private void PrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }

        private void ReadAndValidateInput()
        {
            var read = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(read))
            {
                Self.Tell(new Messages.NullInputError("No input received."));
            }
            else if (string.Equals(read, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                Context.System.Terminate();
            }
            else
            {
                var valid = IsValid(read);
                if (valid)
                {
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you!"));
                    Self.Tell(new Messages.ContinueProcessing());
                }
                else
                {
                    Self.Tell(new Messages.ValidationError("Inalid: Input had odd number of chars."));
                }
            }
            
            bool IsValid(string checking) => checking.Length % 2 == 0;
        }
    }
}
