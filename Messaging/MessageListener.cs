using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Messaging
{
    /// <summary>
    /// Event arguments containing Exception thrown by MessageListener when an IO error is encountered.
    /// </summary>
    public class MessageListenerErrorEventArgs : EventArgs
    {
        internal MessageListenerErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }

        /// <summary>
        /// Gets the Exception thrown by the MessageListener.
        /// </summary>
        public Exception Exception { get; internal set; }
    }


    /// <summary>
    /// Listens for incomming messages on a stream and deserializes to correct type.
    /// </summary>
    internal class MessageListener
    {
        private Stream _stream;
        private IDeserializableMessageFactory _messageDeserializerFactory;
        private MsgListenerState _listenerState;

        private IDeserializableMessage _currentMsg;
        private StringBuilder _currentMsgString;
        private int _currentPropertyIndex;
        private readonly char _msg_start;
        private readonly char _msg_end;
        private readonly char _msg_property;

        readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Event is triggered when an IO exception occurs. The event handler will likely be
        /// executed on a background thread.
        /// </summary>
        public event EventHandler<MessageListenerErrorEventArgs> IOErrorOccurred;

        protected virtual void OnIOErrorOccurred(MessageListenerErrorEventArgs e)
        {
            IOErrorOccurred?.Invoke(this, e);
        }

        /// <summary>
        /// Creates instance with stream and a deserializable message factory.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="messageDeserializerFactory"></param>
        public MessageListener(Stream stream, IDeserializableMessageFactory messageDeserializerFactory)
        : this(stream, messageDeserializerFactory, Convert.ToChar(0x01), Convert.ToChar(0x04), Convert.ToChar(0x02))
        {

        }

        /// <summary>
        /// Creates instance with stream, a deserializable message factory and message delimiter characters.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="messageDeserializerFactory"></param>
        /// <param name="msg_start"></param>
        /// <param name="msg_end"></param>
        /// <param name="msg_property"></param>
        public MessageListener(Stream stream, IDeserializableMessageFactory messageDeserializerFactory,
            char msg_start, char msg_end, char msg_property)
        {
            _stream = stream;
            _messageDeserializerFactory = messageDeserializerFactory;
            _currentMsg = null;
            _currentMsgString = new StringBuilder();
            _currentPropertyIndex = 0;
            _msg_start = msg_start;
            _msg_end = msg_end;
            _msg_property = msg_property;
        }


        /// <summary>
        /// Processes the message stream, listening for incomming messages, then deserializes them.
        /// </summary>
        /// <returns></returns>
        public async Task ProcessMessageStream()
        {
            var buffer = new byte[512];
            int readBytes;

            while (!_cancelTokenSource.IsCancellationRequested)
            try
            {
                using (MemoryStream memStream = new MemoryStream(buffer.Length))
                using (StreamReader streamReader = new StreamReader(memStream))
                {
                    while (!_cancelTokenSource.IsCancellationRequested)
                    {
                        memStream.SetLength(0);
                        streamReader.DiscardBufferedData();

                        readBytes = await _stream.ReadAsync(buffer, 0, buffer.Length, _cancelTokenSource.Token);

                        memStream.Write(buffer, 0, readBytes);
                        memStream.Position = 0;

                        while (streamReader.Peek() >= 0)
                        {
                            char chr = Convert.ToChar(streamReader.Read());

                            switch (_listenerState)
                            {
                                case MsgListenerState.Error:
                                case MsgListenerState.Start:
                                    if (chr == _msg_start)
                                    {
                                        _currentMsgString.Clear();
                                        _listenerState = MsgListenerState.MsgType;
                                    }
                                    else
                                    {
                                        _listenerState = MsgListenerState.Start;
                                    }
                                    break;
                                case MsgListenerState.MsgType:
                                    if (char.IsLetterOrDigit(chr))
                                    {
                                        _currentMsgString.Append(chr);
                                        _listenerState = MsgListenerState.MsgType;
                                    }
                                    else
                                    {
                                        _currentPropertyIndex = 0;
                                        _currentMsg = _messageDeserializerFactory.GetMessageDeserializer(_currentMsgString.ToString());
                                        if (_currentMsg != null)
                                            _currentMsg.OnBeginDeserialize();

                                        if (chr == _msg_property)
                                        {
                                            _listenerState = MsgListenerState.Property;
                                        }
                                        else if (chr == _msg_end)
                                        {
                                            EndDeserialize(_currentMsg, true);
                                            _listenerState = MsgListenerState.Start;
                                        }
                                        else if (chr == _msg_start)
                                        {
                                            EndDeserialize(_currentMsg, false);
                                            _listenerState = MsgListenerState.MsgType;
                                        }
                                        else
                                        {
                                            EndDeserialize(_currentMsg, false);
                                            _listenerState = MsgListenerState.Error;
                                        }
                                        _currentMsgString.Clear();
                                    }
                                    break;
                                case MsgListenerState.Property:
                                    if (chr == _msg_property)
                                    {
                                        DeserializeProperty(_currentMsg, _currentMsgString.ToString());
                                        _listenerState = MsgListenerState.Property;
                                    }
                                    else if (chr == _msg_end)
                                    {
                                        DeserializeProperty(_currentMsg, _currentMsgString.ToString());
                                        EndDeserialize(_currentMsg, true);
                                        _listenerState = MsgListenerState.Start;
                                    }
                                    else if (chr == _msg_start)
                                    {
                                        DeserializeProperty(_currentMsg, _currentMsgString.ToString());
                                        EndDeserialize(_currentMsg, false);
                                        _listenerState = MsgListenerState.MsgType;
                                    }
                                    else
                                    {
                                        _currentMsgString.Append(chr);
                                        _listenerState = MsgListenerState.Property;
                                    }

                                    if (chr == _msg_property || chr == _msg_end || chr == _msg_start)
                                    {
                                        _currentMsgString.Clear();
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                // swallow exception.
            }
            catch (Exception ex)
            {
                OnIOErrorOccurred(new MessageListenerErrorEventArgs(ex));
                Thread.Sleep(5);
            }
        }

        /// <summary>
        /// Cancels the processing of the message stream.
        /// </summary>
        public void Cancel()
        {
            _cancelTokenSource.Cancel();
        }

        /// <summary>
        /// Returns true if currently deserializing a message.
        /// </summary>
        /// <returns></returns>
        public bool IsDeserializingMessage()
        {
            return (_listenerState == MsgListenerState.MsgType || _listenerState == MsgListenerState.Property);
        }

        /// <summary>
        /// Deserializes the property of an incomming message.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="propertyEncValue"></param>
        private void DeserializeProperty(IDeserializableMessage msg, string propertyEncValue)
        {
            if (msg == null)
                return;
            try
            {
                msg.OnDeserializeProperty(_currentPropertyIndex, propertyEncValue);
            }
            catch (Exception ex)
            {
                // Swallow the exception. TODO perhaps log the error somewhere.
            }
            _currentPropertyIndex++;
        }

        /// <summary>
        /// Called when incomming message deserialization is complete or a new incomming message is detected.
        /// Parameter messageComplete is set to true upon completion, and set to false if the
        /// message 'end' delimiter was not encountered.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="messageComplete"></param>
        private static void EndDeserialize(IDeserializableMessage msg, bool messageComplete)
        {
            if (msg != null) msg.OnEndDeserialize(messageComplete);
        }

    }

}
