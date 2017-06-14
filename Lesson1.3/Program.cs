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
            
            var validationActorProps = Props.Create(() => new Actors.ValidationActor(consoleWriterActor));
            var validationActor = actorSystem.ActorOf(validationActorProps, "ValidationActor");

            var consoleReaderProps = Props.Create(() => new Actors.ConsoleReaderActor(validationActor));
            var consoleReaderActor = actorSystem.ActorOf(consoleReaderProps, "ConsoleReaderActor");

            consoleReaderActor.Tell(Actors.ConsoleReaderActor.StartCommand);

            actorSystem.WhenTerminated.Wait();
        }
    }
}
