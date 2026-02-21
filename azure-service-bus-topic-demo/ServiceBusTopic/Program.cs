
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ServiceBusTopic;

string topicName = "apptopic";
string connectionString = "";


List<Order> orders = new List<Order>();
orders.Add(new Order { Id = 1, CourseName = "AZ-204 Developer", Price = 10.99m });
orders.Add(new Order { Id = 2, CourseName = "AZ-104 Azure Administrator", Price = 11.99m });
orders.Add(new Order { Id = 3, CourseName = "DP-203 Azure Data Engineer", Price = 12.99m });
orders.Add(new Order { Id = 4, CourseName = "Test Message 1", Price = 12.99m });
orders.Add(new Order { Id = 31, CourseName = "Test Message 2", Price = 12.99m });
orders.Add(new Order { Id = 43, CourseName = "Test Message 3", Price = 12.99m });
orders.Add(new Order { Id = 34, CourseName = "Test Message 4", Price = 12.99m });

await SendMessages(orders);

// await PeekMessages(3);

async Task SendMessages(List<Order> orders)
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(topicName);

    using ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
    {
        var i = 0;
        foreach (Order order in orders)
        {
            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(order));
            serviceBusMessage.ContentType = "application/json";
            // serviceBusMessage.ApplicationProperties.Add("region", "US");
            if (i % 2 == 0)
            {
                serviceBusMessage.ApplicationProperties.Add("region", "US");
            }
            else
            {
                serviceBusMessage.ApplicationProperties.Add("region", "EU");
            }
            // serviceBusMessage.ApplicationProperties.Add("region", "EU");

            i++;

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
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(topicName, "SubscriptionB");

    IReadOnlyList<ServiceBusReceivedMessage> PeekMessages =
    await serviceBusReceiver.PeekMessagesAsync(maxMessages: numberOfMessages);

    foreach (ServiceBusReceivedMessage serviceBusReceivedMessage in PeekMessages)
    {
        Console.WriteLine($"Message Id {serviceBusReceivedMessage.MessageId}");
        Console.WriteLine($"Message Body {serviceBusReceivedMessage.Body}");
    }

}