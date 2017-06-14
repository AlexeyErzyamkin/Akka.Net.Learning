using System;
using Akka.Actor;

namespace Lesson.Actors
{
    class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        public const string StartCommand = "start";

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case StartCommand:
                {
                    PrintInstructions();
                    break;
                }
            }
            
            ReadAndValidateInput();
        }

        private void PrintInstructions()
        {
            Console.WriteLine("Please provide the URI of a log file on fisk.\n");
        }

        private void ReadAndValidateInput()
        {
            var message = @"C:\Users\Alexey\Documents\test.txt"; //Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(message) && string.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                Context.System.Terminate();
                return;
            }
            
            Context.ActorSelection("akka://Lesson/user/validator").Tell(message);
        }
    }
}
