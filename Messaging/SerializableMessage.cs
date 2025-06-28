using System;
using System.IO;

namespace Messaging
{
    /// <summary>
    /// An abstract class that supports serialization of simple properties
    /// for the purpose of communication over Serial.
    /// </summary>
    internal abstract class SerializableMessage
    {
        volatile private StreamWriter _stream;
        volatile private bool _isSerializing = false;
        volatile private bool _cancel = false;

        private readonly char MSG_START = Convert.ToChar(0x01);
        private readonly char MSG_END = Convert.ToChar(0x04);
        private readonly char MSG_PROPERTY = Convert.ToChar(0x02);

        private static Object lockObj = new Object();

        protected SerializableMessage(StreamWriter stream)
        {
            _stream = stream;
        }

        protected SerializableMessage()
        {

        }

        /// <summary>
        /// Returns the message type string. Must be unique for each implementation.
        /// </summary>
        /// <returns></returns>
        public abstract string GetMessageType();

        /// <summary>
        /// Serializes object to the stream. This method is thread safe.
        /// </summary>
        public virtual void Serialize()
        {
            lock (lockObj)
            {
                _isSerializing = true;
                OnBeginSerialize();
                _stream.Write(MSG_START);
                _stream.Write(GetMessageType());
                OnSerialize();
                _stream.Write(MSG_END);
                _stream.Flush();
                _isSerializing = false;
            }
        }

        /// <summary>
        /// Returns true of the object is currently serializing.
        /// </summary>
        /// <returns></returns>
        public bool IsSerializing()
        {
            return _isSerializing;
        }

        /// <summary>
        /// Signals cancellation of serialization. Subclass implementation
        /// must decide how to cancel serialization.
        /// </summary>
        public void Cancel()
        {
            _cancel = true;
        }

        /// <summary>
        /// Returns true if the cancel method has been invoked.
        /// </summary>
        /// <returns></returns>
        public bool IsCancelled()
        {
            return _cancel;
        }

        /// <summary>
        /// Called when serialization has begun.
        /// </summary>
        protected virtual void OnBeginSerialize()
        {
        }

        /// <summary>
        /// An abstract method that serializes the object.
        /// </summary>
        protected abstract void OnSerialize();

        /// <summary>
        /// Serializes a property to the stream. Set the propertyValue
        /// to an empty string and use the underlying stream if more
        /// formatting control of the property value is required.
        /// </summary>
        /// <param name="propertyValue"></param>
        protected void SerializeProperty(string propertyValue)
        {
            _stream.Write(MSG_PROPERTY);
            _stream.Write(propertyValue);
        }

        protected void SetStream(StreamWriter stream)
        {
            _stream = stream;
        }
        protected StreamWriter GetStream()
        {
            return _stream;
        }
    }

}
