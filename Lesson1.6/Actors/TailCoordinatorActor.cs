using System;
using Akka.Actor;

namespace Lesson.Actors
{
    class TailCoordinatorActor : UntypedActor
    {
        public class StartTail
        {
            public StartTail(string filePath, IActorRef reporterActor)
            {
                FilePath = filePath;
                ReporterActor = reporterActor;
            }

            public string FilePath { get; }
            public IActorRef ReporterActor { get; }
        }

        public class StopTail
        {
            public StopTail(string filePath)
            {
                FilePath = filePath;
            }

            public string FilePath { get; }
        }

        protected override void OnReceive(object message)
        {
            if (message is StartTail start)
            {
                Context.ActorOf(Props.Create(() => new TailActor(start.FilePath, start.ReporterActor)));
            }
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 10,
                withinTimeRange: TimeSpan.FromSeconds(30), 
                localOnlyDecider: x =>
                {
                    switch (x)
                    {
                        case ArithmeticException ex:
                            return Directive.Resume;

                        case NotSupportedException ex:
                            return Directive.Stop;

                        default: return Directive.Restart;
                    }
                }
            );
        }
    }
}
