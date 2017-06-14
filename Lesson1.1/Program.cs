using System;
using Akka.Actor;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("Lesson1");

            var consoleWriterActor = actorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()), "writer");
            var consoleReaderActor = actorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriterActor)), "reader");

            consoleReaderActor.Tell("start");

            actorSystem.WhenTerminated.Wait();
        }
    }
}
