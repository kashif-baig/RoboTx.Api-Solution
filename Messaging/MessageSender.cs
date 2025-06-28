using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Messaging
{
    internal class MessageSender
    {
        static readonly System.Object _lockObject = new object();
        readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        readonly Queue<SerializableMessage> _msgQueue = new Queue<SerializableMessage>();

        public void EnQueueMessage(SerializableMessage msg)
        {
            EnQueueMessage(msg, false);
        }

        public void EnQueueMessage(SerializableMessage msg, bool enqueueDistinctMessageType)
        {
            bool waiting;
            do
            {
                lock (_lockObject)
                {
                    if (_msgQueue.Count() < 6)
                    {
                        if (!(enqueueDistinctMessageType && MessageTypeCount(msg.GetMessageType()) > 0))
                        {
                            _msgQueue.Enqueue(msg);
                        }
                        waiting = false;
                    }
                    else
                    {
                        waiting = true;
                    }
                }
                if (waiting)
                {
                    //await Task.Delay(5);
                    Thread.Sleep(10);
                }
            } while (waiting);
        }

        protected int MessageTypeCount(string messageType)
        {
            int count = 0;
            foreach (var message in _msgQueue)
            {
                if (message.GetMessageType() == messageType)
                {
                    count++;
                }
            }
            return count;
        }

        public /*async Task*/ void ProcessMessageQueue()
        {
            SerializableMessage msg;
            while (!_cancelTokenSource.IsCancellationRequested)
            {
                lock (_lockObject)
                {
                    msg = null;
                    if (_msgQueue.Count() > 0)
                    {
                        msg = _msgQueue.Dequeue();
                    }
                }
                if (msg != null)
                {
                    try
                    {
                        msg.Serialize();
                        //await Task.Delay(10);
                    }
                    catch
                    {
                        // Swallow the error and allow communication to
                        // enter break state.
                    }
                }
                else
                {
                    //await Task.Delay(10);
                    Thread.Sleep(10);
                }
            }
        }

        /// <summary>
        /// Cancels the processing of the message queue.
        /// </summary>
        public void Cancel()
        {
            _cancelTokenSource.Cancel();
        }

    }
}
