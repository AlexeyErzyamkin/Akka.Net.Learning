using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

using Akka.Actor;

namespace Lesson.Actors
{
    class PerformanceCounterCoordinatorActor : ReceiveActor
    {
        public class Watch
        {
            public Watch(CounterType counter)
            {
                Counter = counter;
            }

            public CounterType Counter { get; }
        }

        public class Unwatch
        {
            public Unwatch(CounterType counter)
            {
                Counter = counter;
            }

            public CounterType Counter { get; }
        }

        private static readonly Dictionary<CounterType, Func<PerformanceCounter>> CounterGenerators = new Dictionary<CounterType, Func<PerformanceCounter>>()
        {
            {CounterType.Cpu, () =>
                new PerformanceCounter("Processor", "% Processor Time", "_Total", true)
            },
            {CounterType.Memory, () =>
                new PerformanceCounter("Memory", "% Committed Bytes In Use", true)
            },
            {CounterType.Disk, () =>
                new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total", true)
            }
        };

        private static readonly Dictionary<CounterType, Func<Series>> CounterSeries = new Dictionary<CounterType, Func<Series>>()
        {
            {CounterType.Cpu, () =>
                new Series(CounterType.Cpu.ToString())
                {
                    ChartType = SeriesChartType.SplineArea,
                    Color = Color.DarkGreen
                }
            },
            {CounterType.Memory, () =>
                new Series(CounterType.Memory.ToString())
                {
                    ChartType = SeriesChartType.FastLine,
                    Color = Color.MediumBlue
                }
            },
            {CounterType.Disk, () =>
                new Series(CounterType.Disk.ToString())
                {
                    ChartType = SeriesChartType.FastLine,
                    Color = Color.DarkRed
                }
            }
        };

        private readonly Dictionary<CounterType, IActorRef> _counterActors;
        private readonly IActorRef _chartingActor;

        public PerformanceCounterCoordinatorActor(IActorRef chartingActor)
            : this(chartingActor, new Dictionary<CounterType, IActorRef>())
        {
            _chartingActor = chartingActor;
        }

        public PerformanceCounterCoordinatorActor(IActorRef chartingActor, Dictionary<CounterType, IActorRef> counterActors)
        {
            _counterActors = counterActors;
            _chartingActor = chartingActor;

            Receive<Watch>(
                watch =>
                {
                    if (!_counterActors.ContainsKey(watch.Counter))
                    {
                        var actor = Context.ActorOf(Props.Create(() =>
                            new PerformanceCounterActor(watch.Counter.ToString(), CounterGenerators[watch.Counter])));

                        _counterActors.Add(watch.Counter, actor);
                    }

                    _chartingActor.Tell(new ChartingActor.AddSeries(CounterSeries[watch.Counter]()));
                    _counterActors[watch.Counter].Tell(new SubscribeCounter(watch.Counter, _chartingActor));
                });

            Receive<Unwatch>(
                unwatch =>
                {
                    if (!_counterActors.TryGetValue(unwatch.Counter, out var actor))
                    {
                        return;
                    }
                    
                    actor.Tell(new UnsubscribeCounter(unwatch.Counter, _chartingActor));
                    _chartingActor.Tell(new ChartingActor.RemoveSeries(unwatch.Counter.ToString()));
                });
        }
    }
}
