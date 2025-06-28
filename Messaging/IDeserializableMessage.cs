namespace Messaging
{
    /// <summary>
    /// Defines a deserializable message,
    /// for the purpose of communication over Serial.
    /// </summary>
    internal interface IDeserializableMessage
    {
        /// <summary>
        /// Returns the message type string. Must be unique for each implementation.
        /// </summary>
        /// <returns></returns>
        string GetMessageType();

        /// <summary>
        /// Called when deserialization has begun.
        /// </summary>
        void OnBeginDeserialize();

        /// <summary>
        /// Called when a message property has been deserialized.
        /// </summary>
        /// <param name="propertyIndex"></param>
        /// <param name="propertyValue"></param>
        void OnDeserializeProperty(int propertyIndex, string propertyValue);

        /// <summary>
        /// Called when message serialization has ended.
        /// Parameter messageComplete is set to true upon completion, and set to false if the
        /// message 'end' delimiter was not encountered.
        /// </summary>
        /// <param name="messageComplete"></param>
        void OnEndDeserialize(bool messageComplete);
    }

}
