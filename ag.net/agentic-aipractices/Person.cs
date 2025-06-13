using Json.Schema.Generation;

namespace AgenticAIPractices;

[Title("Person")]
[Description("Represents a person with various attributes such as name, age, email, location, occupation, and hobbies.")]
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string Location { get; set; } = "Unknown";
    public string Occupation { get; set; } = "Unemployed";
    public List<string> Hobbies { get; set; } = new List<string>();

    public override string ToString()
    {
        return @$"Name: {Name}, Age: {Age}, Email: {Email}, 
            Location: {Location}, Occupation: {Occupation}, Hobbies: {string.Join(", ", Hobbies)}";
    }
}