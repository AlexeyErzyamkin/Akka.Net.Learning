﻿using System;
using Akka.Actor;

namespace Lesson.Actors
{
    class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        public const string StartCommand = "start";

        private readonly IActorRef _validationActor;

        public ConsoleReaderActor(IActorRef validationActor)
        {
            _validationActor = validationActor;
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
            var message = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(message) && string.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                Context.System.Terminate();
                return;
            }
            
            _validationActor.Tell(message);
        }
    }
}
