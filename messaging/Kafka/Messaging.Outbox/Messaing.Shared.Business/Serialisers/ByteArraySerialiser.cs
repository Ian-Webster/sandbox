using Confluent.Kafka;
using Messaing.Shared.Business.Models;
using System.Text.Json;

namespace Messaing.Shared.Business.Serialisers
{
    public class ByteArraySerialiser<TMessage>: 
        ISerializer<TMessage>, IDeserializer<TMessage> where TMessage : MessageBase
    {
        public TMessage? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
                return null;

            return JsonSerializer.Deserialize<TMessage>(data);
        }

        public byte[] Serialize(TMessage data, SerializationContext context)
        {
            return JsonSerializer.SerializeToUtf8Bytes(data);
        }
    }
}
