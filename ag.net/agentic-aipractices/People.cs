using Json.Schema.Generation;

namespace AgenticAIPractices;

[Title("People")]
[Description("Represents a collection of persons, allowing for multiple individuals to be stored and managed.")]
public class People
{
    public List<Person> Persons { get; set; } = new List<Person>();
}