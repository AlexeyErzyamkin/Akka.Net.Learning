using System;
using System.IO;
using System.Text;

using Akka.Actor;

namespace Lesson.Actors
{
    class TailActor : UntypedActor
    {
        public class FileWrite
        {
            public FileWrite(string fileName)
            {
                FileName = fileName;
            }

            public string FileName { get; }
        }

        public class FileError
        {
            public FileError(string fileName, string reason)
            {
                FileName = fileName;
                Reason = reason;
            }

            public string FileName { get; }
            public string Reason { get; }
        }

        public class InitialRead
        {
            public InitialRead(string fileName, string text)
            {
                FileName = fileName;
                Text = text;
            }

            public string FileName { get; }
            public string Text { get; }
        }

        private readonly string _filePath;
        private readonly IActorRef _reporterActor;
        private readonly FileObserver _observer;
        private readonly System.IO.Stream _fileStream;
        private readonly System.IO.StreamReader _fileStreamReader;

        public TailActor(string filePath, IActorRef reporterActor)
        {
            _filePath = filePath;
            _reporterActor = reporterActor;

            var fullPath = System.IO.Path.GetFullPath(_filePath);
            _observer = new FileObserver(Self, fullPath);
            _observer.Start();

            _fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            _fileStreamReader = new StreamReader(_fileStream, Encoding.UTF8);

            var text = _fileStreamReader.ReadToEnd();
            Self.Tell(new InitialRead(_filePath, text));
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case FileWrite write:
                {
                    var text = _fileStreamReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(text))
                    {
                        _reporterActor.Tell(text);
                    }
                    break;
                }

                case FileError error:
                {
                    _reporterActor.Tell($"Tail error: '{error.Reason}'");
                    break;
                }

                case InitialRead read:
                {
                    _reporterActor.Tell(read.Text);
                    break;
                }
            }
        }
    }
}
