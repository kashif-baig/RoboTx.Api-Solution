namespace Messaging
{
    /// <summary>
    /// Defines a factory for deserializable messages.
    /// For use by the MessageListener.
    /// </summary>
    internal interface IDeserializableMessageFactory
    {
        /// <summary>
        /// Returns a message instance, given a message type string.
        /// Returns NULL if message type is not recognised.
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        IDeserializableMessage GetMessageDeserializer(string messageType);
    }

}
