using System;
using System.Collections.Generic;
using System.Diagnostics;

using Akka.Actor;

namespace Lesson.Actors
{
    class PerformanceCounterActor : UntypedActor
    {
        private readonly string _seriesName;
        private readonly Func<PerformanceCounter> _generator;
        private PerformanceCounter _counter;

        private readonly HashSet<IActorRef> _subscriptions;
        private readonly ICancelable _cancelPublishing;

        public PerformanceCounterActor(string seriesName, Func<PerformanceCounter> generator)
        {
            _seriesName = seriesName;
            _generator = generator;

            _subscriptions = new HashSet<IActorRef>();
            _cancelPublishing = new Cancelable(Context.System.Scheduler);
        }

        protected override void PreStart()
        {
            _counter = _generator();
            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromMilliseconds(250),
                TimeSpan.FromMilliseconds(250),
                Self,
                new GatherMetrics(),
                Self,
                _cancelPublishing
            );
        }

        protected override void PostStop()
        {
            try
            {
                _cancelPublishing.Cancel(false);
                _counter?.Dispose();
            }
            catch
            {
            }
            finally
            {
                base.PostStop();
            }
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case GatherMetrics gm:
                {
                    var metric = new Metric(_seriesName, _counter.NextValue());
                    foreach (var eachSubscription in _subscriptions)
                    {
                        eachSubscription.Tell(metric);
                    }
                    break;
                }

                case SubscribeCounter sc:
                {
                    _subscriptions.Add(sc.Subscriber);
                    break;
                }

                case UnsubscribeCounter uc:
                {
                    _subscriptions.Remove(uc.Subscriber);
                    break;
                }
            }
        }
    }
}
