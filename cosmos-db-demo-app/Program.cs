using cosmosdb_app;
using Microsoft.Azure.Cosmos;


List<Student> stdList = new List<Student>()
{
    new Student{ studentId = "Ali", price=10},
    // new Student{ "James", 35},
    // new Student{ "Que", 81}
};


List<Student> studentsEnrolled = new List<Student>();
// Add students to the list if needed
studentsEnrolled.Add(new Student("Alice", 1));
studentsEnrolled.Add(new Student("Bob", 2));

//await CreateCourse(new Course("AZ-104 Azure Administrator", 4.7, "Certification", [1, 2, 3], studentsEnrolled));

//await CreateCourse(new Course("Learning Kubernetes", 4.6, "Software", [22, 23, 12], stdList));

// await CreateCourse(new Course("AZ-204 Azure Developer", 4.8, "Certification"));


await DisplayAllCourses();


CosmosClient ConnectDatabase()
{
    string connectionString = "AccountEndpoint=https://mobeendev.documents.azure.com:443/;AccountKey=iaMeZqrqgoAHeQB1SyeZkKZQR9gwXe7m1KrLIT3lDXjTDkfZgeF0yPRXqcBQlh48TGGEh2OlL3jjACDbZeMwkA==;";
    return new CosmosClient(connectionString);

}

async Task CreateCourse(Course course)
{
    CosmosClient cosmosClient = ConnectDatabase();
    string databaseId = "appdb";
    string containerId = "courses";

    Database database = cosmosClient.GetDatabase(databaseId);
    Container container = database.GetContainer(containerId);

    ItemResponse<Course> item = await container.CreateItemAsync<Course>(course, new PartitionKey(course.category));
    Console.WriteLine(item.StatusCode);
}

async Task DisplayAllCourses()
{
    QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM courses c");
    string databaseId = "appdb";
    string containerId = "courses";
    CosmosClient client = ConnectDatabase();
    Database database = client.GetDatabase(databaseId);
    Container container = database.GetContainer(containerId);

    using (FeedIterator<Course> feedIterator = container.GetItemQueryIterator<Course>(queryDefinition))
    {
        while (feedIterator.HasMoreResults)
        {
            FeedResponse<Course> response = await feedIterator.ReadNextAsync();
            foreach (var item in response)
            {
                Console.WriteLine($"Course ID : {item.id}");
                Console.WriteLine($"Course Name : {item.name}");
                Console.WriteLine($"Course Rating : {item.rating}");
                Console.WriteLine($"Course Category : {item.category}");
                Console.WriteLine("Public Chapters");
                // For arrays, use Length (not Count)
                if (item.publicChapters != null && item.publicChapters.Length > 0)
                {
                    Console.WriteLine("Public Chapters:");
                    foreach (int chapter in item.publicChapters)
                        Console.Write($"{chapter} ");
                    Console.WriteLine(); // Add line break after chapters
                }
                // For Lists, use Count
                if (item.students != null && item.students.Count > 0)
                {
                    Console.WriteLine("Student Information:");
                    foreach (Student student in item.students)
                    {
                        Console.WriteLine($"Student ID : {student.studentId}");
                        Console.WriteLine($"Price : {student.price}");
                    }
                }
            }
        }
    }

}