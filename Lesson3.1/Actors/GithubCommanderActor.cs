using System;
using System.Linq;
using Akka.Actor;
using Akka.Routing;

namespace GithubActors.Actors
{
    /// <summary>
    /// Top-level actor responsible for coordinating and launching repo-processing jobs
    /// </summary>
    public class GithubCommanderActor : ReceiveActor, IWithUnboundedStash
    {
        #region Message classes

        public class CanAcceptJob
        {
            public CanAcceptJob(RepoKey repo)
            {
                Repo = repo;
            }

            public RepoKey Repo { get; private set; }
        }

        public class AbleToAcceptJob
        {
            public AbleToAcceptJob(RepoKey repo)
            {
                Repo = repo;
            }

            public RepoKey Repo { get; private set; }
        }

        public class UnableToAcceptJob
        {
            public UnableToAcceptJob(RepoKey repo)
            {
                Repo = repo;
            }

            public RepoKey Repo { get; private set; }
        }

        #endregion

        private IActorRef _coordinator;
        private IActorRef _canAcceptJobSender;
        private int _pendingJobReplies;

        public GithubCommanderActor()
        {
            ReadyState();
        }

        public IStash Stash { get; set; }

        private void ReadyState()
        {
            Receive<CanAcceptJob>(job =>
            {
                _coordinator.Tell(job);

                BecomeAsking();
            });
        }

        private void AskingState()
        {
            Receive<CanAcceptJob>(job => Stash.Stash());

            Receive<UnableToAcceptJob>(job =>
            {
                _pendingJobReplies--;
                if (_pendingJobReplies <= 0)
                {
                    _canAcceptJobSender.Tell(job);

                    BecomeReady();
                }
            });

            Receive<AbleToAcceptJob>(job =>
            {
                _canAcceptJobSender.Tell(job);
                _coordinator.Tell(new GithubCoordinatorActor.BeginJob(job.Repo));

                Context.ActorSelection(ActorPaths.MainFormActor.Path).Tell(new MainFormActor.LaunchRepoResultsWindow(job.Repo, Sender));

                BecomeReady();
            });
        }

        private void BecomeReady()
        {
            Become(ReadyState);
            Stash.UnstashAll();
        }

        private void BecomeAsking()
        {
            _canAcceptJobSender = Sender;
            _pendingJobReplies = 3;
            Become(AskingState);
        }

        protected override void PreStart()
        {
            var name1 = ActorPaths.GithubCoordinatorActor.Name + 1;
            var name2 = ActorPaths.GithubCoordinatorActor.Name + 2;
            var name3 = ActorPaths.GithubCoordinatorActor.Name + 3;

            var ca1 = Context.ActorOf(Props.Create(() => new GithubCoordinatorActor()), name1);
            var ca2 = Context.ActorOf(Props.Create(() => new GithubCoordinatorActor()), name2);
            var ca3 = Context.ActorOf(Props.Create(() => new GithubCoordinatorActor()), name3);

            _coordinator = Context.ActorOf(
                Props.Empty.WithRouter(
                    new BroadcastGroup(name1, name2, name3)
                ));

            base.PreStart();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            //kill off the old coordinator so we can recreate it from scratch
            _coordinator.Tell(PoisonPill.Instance);
            base.PreRestart(reason, message);
        }
    }
}
