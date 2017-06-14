using System;
using Akka.Actor;

namespace Lesson
{
    class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            var msg = message as string;

            if (string.IsNullOrEmpty(msg))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Please provide an input.\n");
                Console.ResetColor();

                return;
            }

            var even = msg.Length % 2 == 0;
            var color = even ? ConsoleColor.Red : ConsoleColor.Green;
            var alert = even ? "Your string had an even # of characters.\n" : "Your string had an odd # of characters.\n";

            Console.ForegroundColor = color;
            Console.WriteLine(alert);
            Console.ResetColor();
        }
    }
}
