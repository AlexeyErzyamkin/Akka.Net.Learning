using System;
using System.Windows.Forms;

using Akka.Actor;

namespace Lesson.Actors
{
    class ButtonToggleActor : UntypedActor
    {
        public class Toggle { }

        private readonly IActorRef _coordinatorActor;
        private readonly Button _button;
        private readonly CounterType _counterType;
        private bool _isToggleOn;

        public ButtonToggleActor(IActorRef coordinatorActor, Button button, CounterType counterType, bool isToggleOn)
        {
            _coordinatorActor = coordinatorActor;
            _button = button;
            _counterType = counterType;
            _isToggleOn = isToggleOn;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case Toggle x when (_isToggleOn):
                    _coordinatorActor.Tell(new PerformanceCounterCoordinatorActor.Unwatch(_counterType));
                    FlipToggle();
                    break;

                case Toggle x when !_isToggleOn:
                    _coordinatorActor.Tell(new PerformanceCounterCoordinatorActor.Watch(_counterType));
                    FlipToggle();
                    break;

                default:
                    Unhandled(message);
                    break;
            }
        }

        private void FlipToggle()
        {
            _isToggleOn = !_isToggleOn;

            _button.Text = _counterType + " (" + (_isToggleOn ? "ON" : "OFF") + ")";
        }
    }
}
