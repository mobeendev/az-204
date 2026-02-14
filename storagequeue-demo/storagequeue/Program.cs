// First add the package - dotnet add package Azure.Storage.Queues
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

string queueName = "appqueue";
string connectionString = "";

for (int i = 0; i < 5; i++)
    await AddMessage($"Test Message {i}");

// await PeekMessage(2);
// await ReceiveMessage(7);
// GetQueueLength();

async Task AddMessage(string message)
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    await queueClient.SendMessageAsync(message);
    Console.WriteLine($"Message added to queue - {message}");
}

async Task PeekMessage(int messageCount)
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    PeekedMessage[] messages = await queueClient.PeekMessagesAsync(maxMessages: messageCount);

    foreach (PeekedMessage message in messages)
    {
        Console.WriteLine($"Message ID #{message.MessageId}");
        Console.WriteLine($"Message Body #{message.Body}");
    }

}

async Task ReceiveMessage(int messageCount)
{
    QueueClient queueClient = new QueueClient(connectionString, queueName);
    QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(maxMessages: messageCount);
    // Receiving the messages will make the messages invisible 
    // During this time you can process the messages
    // To delete the message , we need to delete them specifically

    // This value is required to delete the Message. If deletion fails using this popreceipt then the message has been dequeued by another client.
    foreach (QueueMessage message in messages)
    {
        Console.WriteLine($"Deleting message with Message ID #{message.MessageId}");
        await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
    }

}




//-----------------------------------------------------
// Get the approximate number of messages in the queue
//-----------------------------------------------------
void GetQueueLength()
{

    QueueClient queueClient = new QueueClient(connectionString, queueName);
    if (queueClient.Exists())
    {
        QueueProperties properties = queueClient.GetProperties();

        // Retrieve the cached approximate message count.
        int cachedMessagesCount = properties.ApproximateMessagesCount;

        // Display number of messages.
        Console.WriteLine($"Number of messages in queue: {cachedMessagesCount}");
    }
}
