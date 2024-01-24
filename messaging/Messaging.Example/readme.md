# Messaging.Example

## Introduction
A basic example of messaging via [Kafka](https://kafka.apache.org/).

The project consists of a single [producer](https://docs.confluent.io/platform/current/clients/producer.html) and two [consumers](https://docs.confluent.io/platform/current/clients/consumer.html).

The producer produces three message types;
1. "hello all" + a Guid + a date-time stamp
2. "hello consumer 1" + a Guid + a date-time stamp
3. "hello consumer 2" + a Guid + a date-time stamp

Both consumers consume "hello all" and the respective consumer will consumer the "hello consumer X" messages


