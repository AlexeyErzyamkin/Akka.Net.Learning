using System;
using System.Collections.Generic;

using Akka.Actor;

namespace Lesson.Actors
{
    class GatherMetrics { }

    class Metric
    {
        public Metric(string series, float counterValue)
        {
            Series = series;
            CounterValue = counterValue;
        }

        public string Series { get; }
        public float CounterValue { get; }
    }

    enum CounterType
    {
        Cpu,
        Memory,
        Disk
    }

    class SubscribeCounter
    {
        public SubscribeCounter(CounterType counter, IActorRef subscriber)
        {
            Counter = counter;
            Subscriber = subscriber;
        }

        public CounterType Counter { get; }
        public IActorRef Subscriber { get; }
    }

    class UnsubscribeCounter
    {
        public UnsubscribeCounter(CounterType counter, IActorRef subscriber)
        {
            Counter = counter;
            Subscriber = subscriber;
        }

        public CounterType Counter { get; }
        public IActorRef Subscriber { get; }
    }
}
