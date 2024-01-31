using Confluent.Kafka;
using Messaging.Example.Business.Models;
using System.Text.Json;

namespace Messaging.Example.Business.Services
{
    public class MessageSerialiser<MessageBase> : ISerializer<MessageBase>, IDeserializer<MessageBase>
    {
        public byte[] Serialize(MessageBase data, SerializationContext context)
        {
            var test = JsonSerializer.Serialize(data);
            return JsonSerializer.SerializeToUtf8Bytes(data);
        }

        public MessageBase Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
                return default;

            return JsonSerializer.Deserialize<MessageBase>(data);
        }
    }
}
