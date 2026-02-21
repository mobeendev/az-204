

using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ServiceBusQueue;

string queueName = "appqueue";
string connectionString = "";

List<Order> orders = new List<Order>();
orders.Add(new Order { Id = 1, CourseName = "AZ-204 Developer", Price = 10.99m });
orders.Add(new Order { Id = 2, CourseName = "AZ-104 Azure Administrator", Price = 11.99m });
orders.Add(new Order { Id = 3, CourseName = "DP-203 Azure Data Engineer", Price = 12.99m });

//await SendMessages(orders);

//await PeekMessages(3);

await ReceiveMessages(6);

async Task SendMessages(List<Order> orders)
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(queueName);

    using ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
    {
        foreach (Order order in orders)
        {
            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(order));
            serviceBusMessage.ContentType = "application/json";
            serviceBusMessage.ApplicationProperties.Add("Month", "January");
            serviceBusMessageBatch.TryAddMessage(serviceBusMessage);
        }
    }

    await serviceBusSender.SendMessagesAsync(serviceBusMessageBatch);
    Console.WriteLine("All Messages sent");
    await serviceBusSender.DisposeAsync();
    await serviceBusClient.DisposeAsync();
}

async Task PeekMessages(int numberOfMessages)
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(queueName);

    IReadOnlyList<ServiceBusReceivedMessage> PeekMessages =
    await serviceBusReceiver.PeekMessagesAsync(maxMessages: numberOfMessages);

    foreach (ServiceBusReceivedMessage serviceBusReceivedMessage in PeekMessages)
    {
        Console.WriteLine($"Message Id {serviceBusReceivedMessage.MessageId}");
        Console.WriteLine($"Message Body {serviceBusReceivedMessage.Body}");
        Console.WriteLine($"Month - {serviceBusReceivedMessage.ApplicationProperties["Month"]}");
    }

}


async Task ReceiveMessages(int numberOfMessages)
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(queueName,
     new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

    IReadOnlyList<ServiceBusReceivedMessage> receivedMessages =
  await serviceBusReceiver.ReceiveMessagesAsync(maxMessages: numberOfMessages);

    foreach (ServiceBusReceivedMessage serviceBusReceivedMessage in receivedMessages)
    {
        Console.WriteLine($"Message Id {serviceBusReceivedMessage.MessageId}");
        Console.WriteLine($"Message Body {serviceBusReceivedMessage.Body}");
    }
}