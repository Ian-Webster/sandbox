# Messaging.Example

## Introduction
A basic example of messaging via [Kafka](https://kafka.apache.org/).

The project consists of a single [producer](https://docs.confluent.io/platform/current/clients/producer.html) and two [consumers](https://docs.confluent.io/platform/current/clients/consumer.html).

The producer produces three message types;
1. "hello all" + a Guid + a date-time stamp
2. "hello consumer 1" + a Guid + a date-time stamp
3. "hello consumer 2" + a Guid + a date-time stamp

Both consumers consume "hello all" and the respective consumer will consume the "hello consumer X" messages

## Kafka set up

This project uses [Kafka](https://kafka.apache.org/) for its message bus, you'll need an instance to run this project.

For the purposes of this document we'll use [Confluents](https://developer.confluent.io/) [local development](https://docs.confluent.io/platform/current/installation/docker/image-reference.html) docker [image](https://hub.docker.com/r/confluentinc/confluent-local)

`docker pull confluentinc/confluent-local`

Once you have your instance running you'll need to confirm it's working correctly, the best approach is to use a IDE, the author has used [Conduktor](https://www.conduktor.io/get-started/#desktop) several times with success but [others are available](https://forum.confluent.io/t/3rd-party-gui-tools-for-apache-kafka/6004).

## Producer set up
A [message producer](https://docs.confluent.io/platform/current/clients/producer.html) creates messages and pushes them into the message bus, it should be noted that a producer can also be a consumer but to keep this example simple our producer will only produce messages.

The following instructions are largely based on [this guide](https://developer.confluent.io/get-started/dotnet/#build-producer)

1. Create a new project for your producer
2. Install the [Confluent.Kafka](https://www.nuget.org/packages/Confluent.Kafka/) NuGet package.
3. Create a model to represent your message, this will be a POCO, for instance;
    ```csharp
    public class HelloAllMessage : MessageBase
    {
        public HelloAllMessage() : base("Hello All")
        {
        }
    }

   public class MessageBase
    {
        protected Guid MessageId { get; set; }
    
        protected string Message { get; set; }
    
        protected DateTime Created { get; set; }
    
        protected MessageBase(string message)
        {
            MessageId = Guid.NewGuid();
            Created = DateTime.Now;
            Message = message;  
        }
    
        public override string ToString()
        {
            return $"{MessageId} - {Message} - {Created}";
        }
    }
    ```
4. Create a producer service;
    ```csharp
    /// <summary>
    /// Sends messages to Kafka
    /// </summary>
    public class ProducerService
    {
        private IProducer<string, MessageBase> producer;
    
        public ProducerService()
        {
            // create configuation for the producer
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            // create the producer
            producer = new ProducerBuilder<string, MessageBase>(producerConfig)
                .SetValueSerializer(new MessageSerialiser()) // because we are sending a custom object we need to tell Kafka how to serialise it
                .Build();
        }
    
        /// <summary>
        /// Sends a message to Kafka on the specified topic
        /// </summary>
        /// <param name="topic">topic to send the message to</param>
        /// <param name="message">message to send</param>
        public void Produce(string topic, MessageBase message)
        {
            producer.Produce(topic, new Message<string, MessageBase>
            {
                Key = message.MessageId.ToString(),
                Value = message
            },
            (deliveryReport) =>
            {
                // handle the result of sending the message
                if (deliveryReport.Error.Code != ErrorCode.NoError)
                    // failure
                    AnsiConsole.MarkupLine($"[bold red]Failed to send message {message} error was {deliveryReport.Error.Code}[/]");
                else
                    // success
                    AnsiConsole.MarkupLine($"[bold green]Successfully sent message {message}[/]");
            });
        }
    
        /// <summary>
        /// Flushes the producer
        /// </summary>
        public void Flush()
        {
            producer.Flush();
        }
    
    }
    ```
5. Write some code to generate messages and send them;
    ```csharp
    private bool SendMessages(MessageTypes messageType, int numberToSend)
    {
        for (int i = 0; i < numberToSend; i++)
        {
            var message = CreateMessage(messageType);
    
            AnsiConsole.MarkupLine($"[bold green]Generated message {message} {i+1} of {numberToSend}[/]");
    
            producerService.Produce("HelloAll", message);
        }
    
        producerService.Flush();
               
        return true;
    }
    ```
6. Run the app, trigger the sending code and use your Kafka IDE to test if the messages were produced on the topic

**note** `AnsiConsole` seen in the above code is from the [Spectre.Console](https://spectreconsole.net/) library, it works in the same ways as `Console.WriteLine()` it just looks fancier.

## Consumer set up
A [message consumer](https://docs.confluent.io/platform/current/clients/consumer.html) reads messages from the message bus, as with a producer it is possible for a consumer to also be a producer however we'll keep it simple for this example

The following instructions are taken mostly from [this guide](https://developer.confluent.io/get-started/dotnet/#build-consumer);

1. Either add to your existing project (created in "Producer set up") or create a new one for your consumer, if you are creating a new one you'll need to install the [Confluent.Kafka](https://www.nuget.org/packages/Confluent.Kafka/) NuGet package and share the message model/s you created for your producer.
2. Create a consumer service;
    ```csharp
    /// <summary>
    /// Reads messages from a Kafka topic
    /// </summary>
    /// <typeparam name="TMessageType"></typeparam>
    public class ConsumerService<TMessageType>: IDisposable where TMessageType : MessageBase
    {
        private IConsumer<string, TMessageType> consumer;
    
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="topic">The topic this consumer should subscribe to</param>
        public ConsumerService(string topic)
        {
            // set up consumer configuration
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
    
            // create the consumer
            consumer = new ConsumerBuilder<string, TMessageType>(consumerConfig)
                .SetValueDeserializer(new MessageSerialiser<TMessageType>()) // because we are sending a custom object we need to tell Kafka how to deserialise it
                .Build();
    
            consumer.Subscribe(topic); // subscribe to the topic
        }
    
        /// <summary>
        /// Consumes a message from Kafka
        /// </summary>
        /// <returns></returns>
        public TMessageType ConsumeMessage()
        {
            try
            {
                var consumeResult = consumer.Consume();
                return consumeResult.Message.Value;
            }
            catch (ConsumeException ex)
            {
                // log the exception
                return default;
            }
        }
    
        public void Dispose()
        {
            consumer.Close();
            consumer.Dispose();
        }
    }
    ```
3. Write code to start consuming;
	```csharp
	public class MessageConsumer
	{
		private ConsumerService<HelloAllMessage> _allConsumer;

		public MessageConsumer()
		{
			_allConsumer = new ConsumerService<HelloAllMessage>("HelloAll");
		}

		public void StartConsuming()
		{
			int counter = 1;
			AnsiConsole.MarkupLine($"[bold teal]Consumer1 listening for messages[/]");
			while (true)
			{
				var message = _allConsumer.ConsumeMessage();
				AnsiConsole.MarkupLine($"[bold teal]Successfully received message {counter} {message}[/]");
				counter++;
			}
		}
	}
	```

## Testing
1. Create a topic named "HelloAll" in Kafka
2. Run the producer and consumer applications
3. Producer some "HelloAll" messages, you should see a "received" for each message you created.

## Modifying the produce

Now we can send the "HelloAll" message we'll make modifications to the producer so it can also send the "HelloConsumer1" and "HellowConsumer2" messages;

1. Add two new topics to your Kafka instances, named "HelloConsumer1" and "HelloConsumer2"
2. Create models for the two messages, in the example we make use of the base message class;
    ```csharp
    public class HelloConsumer1Message : MessageBase
    {
        public HelloConsumer1Message(): base("Hello Consumer 1")
        {
        }
    }

    public class HelloConsumer2Message : MessageBase
    {
        public HelloConsumer2Message(): base("Hello Consumer 2")
        {
        }
    }
    ```
3. Modify the code that produces and sends your messages to produce the new types;
    ```csharp
    private bool SendMessages(MessageTypes messageType, int numberToSend)
    {
        for (int i = 0; i < numberToSend; i++)
        {
            var messageData = CreateMessage(messageType);

            AnsiConsole.MarkupLine($"[bold green]Generated message {messageData.message} {i+1} of {numberToSend}[/]");

            producer.Produce(messageData.topicName, messageData.message);
        }

        producer.Flush();
           
        return true;
    }

   private (MessageBase message, string topicName) CreateMessage(MessageTypes messageType)
    {
        switch (messageType)
        {
            case MessageTypes.HelloAll:
                return (new HelloAllMessage(), "HelloAll");
            case MessageTypes.HelloConsumer1:
                return (new HelloConsumer1Message(), "HelloConsumer1");
            case MessageTypes.HelloConsumer2:
                return (new HelloConsumer2Message(), "HelloConsumer2");
            case MessageTypes.Random:
                {
                    var bogus = new Bogus.Faker();
                    return CreateMessage(bogus.PickRandomWithout(MessageTypes.Random));
                }
            default:
                throw new Exception("Invalid message type");
        }
    }
    ```
4. Create some messages of type "HelloConsumer1" and "HelloConsumer2", use your Kafka IDE to verify the messages are going into the correct topics

## Modifying the consumer
We had previously set up Consumer1 to receive the "HelloAll" messages, both consumers will eventually receive these but to start we'll set finish setting up consumer 1 to receive the "HelloConsumer1" message;
1. 

