using System;
using Akka.Actor;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("Lesson");

            var consoleWriterProps = Props.Create<Actors.ConsoleWriterActor>();
            var consoleWriterActor = actorSystem.ActorOf(consoleWriterProps, "ConsoleWriterActor");

            var tailCoordinator = actorSystem.ActorOf(Props.Create<Actors.TailCoordinatorActor>(), "tail_coordinator");

            var fileValidatorProps = Props.Create(() => new Actors.FileValidatorActor(consoleWriterActor));
            var fileValidator = actorSystem.ActorOf(fileValidatorProps, "validator");

            var consoleReaderProps = Props.Create<Actors.ConsoleReaderActor>();
            var consoleReaderActor = actorSystem.ActorOf(consoleReaderProps, "ConsoleReaderActor");

            consoleReaderActor.Tell(Actors.ConsoleReaderActor.StartCommand);

            actorSystem.WhenTerminated.Wait();
        }
    }
}
