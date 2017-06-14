using System;
using System.IO;

using Akka.Actor;

namespace Lesson
{
    class FileObserver : IDisposable
    {
        private readonly IActorRef _tailActor;
        private readonly string _absoluteFIlePath;
        private readonly string _fileDir;
        private readonly string _fileNameOnly;
        private FileSystemWatcher _watcher;

        public FileObserver(IActorRef tailActor, string absoluteFIlePath)
        {
            _tailActor = tailActor;
            _absoluteFIlePath = absoluteFIlePath;

            _fileDir = Path.GetDirectoryName(absoluteFIlePath);
            _fileNameOnly = Path.GetFileName(absoluteFIlePath);
        }

        public void Start()
        {
            _watcher = new FileSystemWatcher(_fileDir, _fileNameOnly)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            _watcher.Changed += WatcherOnChanged;
            _watcher.Error += WatcherOnError;

            _watcher.EnableRaisingEvents = true;
        }

        public void Dispose()
        {
            _watcher?.Dispose();
        }

        private void WatcherOnError(object sender, ErrorEventArgs errorEventArgs)
        {
            _tailActor.Tell(new Actors.TailActor.FileError(_absoluteFIlePath, errorEventArgs.GetException().Message), ActorRefs.NoSender);
        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            if (fileSystemEventArgs.ChangeType == WatcherChangeTypes.Changed)
            {
                _tailActor.Tell(new Actors.TailActor.FileWrite(_absoluteFIlePath), ActorRefs.NoSender);
            }
        }
    }
}
