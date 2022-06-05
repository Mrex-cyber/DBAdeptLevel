using Microsoft.EntityFrameworkCore;

namespace DBTrainingAdeptLevel
{
    // Many to many
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Course>? Courses { get; set; }
    }
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Student>? Students { get; set; }

    }

    // Connections
    public class City
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    public class Country
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CapitalId { get; set; }
        public City? Capital { get; set; } 
        public List<Company> Companies { get; set; } = new();
    }
    public class Company
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CountryId { get; set; }
        public Country? Country { get; set; }
        public List<User> Users { get; set; } = new();
    }
    public class Position
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<User> Users { get; set; } = new();
    }
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
        public int? PositionId { get; set; }
        public Position? Position { get; set; }
    }
}
