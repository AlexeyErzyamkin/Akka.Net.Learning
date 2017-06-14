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

            var tailCoordinator = actorSystem.ActorOf(Props.Create<Actors.TailCoordinatorActor>(), "TailCoordinator");

            var fileValidatorProps = Props.Create(() => new Actors.FileValidatorActor(consoleWriterActor, tailCoordinator));
            var fileValidator = actorSystem.ActorOf(fileValidatorProps, "FileValidator");

            var consoleReaderProps = Props.Create(() => new Actors.ConsoleReaderActor(fileValidator));
            var consoleReaderActor = actorSystem.ActorOf(consoleReaderProps, "ConsoleReaderActor");

            consoleReaderActor.Tell(Actors.ConsoleReaderActor.StartCommand);

            actorSystem.WhenTerminated.Wait();
        }
    }
}
