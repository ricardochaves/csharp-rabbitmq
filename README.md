# POC: C# and RabbitMQ

## How to use

Clone and `docker-compose up`

Send a message:
```c#
const string queueName = "Queue Test";

var publisher = new RabbitMQPublisher();
publisher.publishStringMessageToQueue("Ricardo Test",queueName);
```
Consume messages:

```c#
private static void PrintMessage(Payload payload)
{
    var bytesAsString = Encoding.UTF8.GetString(payload.GetBody());
    Console.Write(bytesAsString);
    payload.Ack();

}

using var consumer = new RabbitMqConsumer();
consumer.Start(queueName,PrintMessage);
```

## Checklist

https://www.rabbitmq.com/production-checklist.html

## TODO
- Connection recovery will not kick in when a channel is closed due to a channel-level exception. Such exceptions often indicate application-level issues. The library cannot make an informed decision about when that's the case. [source](https://www.rabbitmq.com/dotnet-api-guide.html#topology-recovery)
- Closed channels won't be recovered even after connection recovery kicks in. This includes both explicitly closed channels and the channel-level exception case above. [source](https://www.rabbitmq.com/dotnet-api-guide.html#automatic-recovery-limitations)


## Tips

### We can't use autoAck

> In automatic acknowledgement mode, a message is considered to be successfully delivered immediately after it is sent. This mode trades off higher throughput (as long as the consumers can keep up) for reduced safety of delivery and consumer processing. This mode is often referred to as "fire-and-forget". Unlike with manual acknowledgement model, if consumers's TCP connection or channel is closed before successful delivery, the message sent by the server will be lost. Therefore, automatic message acknowledgement should be considered unsafe and not suitable for all workloads.

[source](https://www.rabbitmq.com/confirms.html#acknowledgement-modes)

### Always use nack

> The AMQP 0-9-1 specification defines the basic.reject method that allows clients to reject individual, delivered messages, instructing the broker to either discard them or requeue them. Unfortunately, basic.reject provides no support for negatively acknowledging messages in bulk.

> To solve this, RabbitMQ supports the basic.nack method that provides all the functionality of basic.reject whilst also allowing for bulk processing of messages.

[source](https://www.rabbitmq.com/nack.html)

### ack and nack or reject message

> Positive acknowledgements simply instruct RabbitMQ to record a message as delivered and can be discarded. Negative acknowledgements with basic.reject have the same effect. The difference is primarily in the semantics: positive acknowledgements assume a message was successfully processed while their negative counterpart suggests that a delivery wasn't processed but still should be deleted.

[source](https://www.rabbitmq.com/confirms.html#acknowledgement-modes)

