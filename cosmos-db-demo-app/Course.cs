namespace cosmosdb_app;

public class Course
{
    public string? id { get; set; }
    public string? name { get; set; }

    public double rating { get; set; }

    public string? category { get; set; }

    public List<Student> students { get; set; }

    public int[] publicChapters { get; set; }

    public Course()
    {

    }

    public Course(string name, double rating, string category, int[] chp, List<Student> std)
    {
        this.id = Guid.NewGuid().ToString();
        this.name = name;
        this.rating = rating;
        this.category = category;
        this.students = std;
        this.publicChapters = chp;
    }
}


public class Student
{
    public string? studentId { get; set; }

    public decimal price { get; set; }
    public Student()
    {

    }

    public Student(string studentId, decimal price)
    {
        this.studentId = studentId;
        this.price = price;
    }
}