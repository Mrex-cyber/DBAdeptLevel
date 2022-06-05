using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DBTrainingAdeptLevel
{
    class Program
    {
        static void Main()
        {
            using (ApplicationContext appContext = new ApplicationContext())
            {                        
                Position manager = new Position { Name = "Manager" };
                Position developer = new Position { Name = "Developer" };
                appContext.Positions.AddRange(manager, developer);

                City washington = new City { Name = "Washington" };
                appContext.Cities.Add(washington);

                Country usa = new Country { Name = "USA", Capital = washington };
                appContext.Countries.Add(usa);

                Company microsoft = new Company { Name = "Microsoft", Country = usa };
                Company google = new Company { Name = "Google", Country = usa };
                appContext.Companies.AddRange(microsoft, google);

                User tom = new User { Name = "Tom", Company = microsoft, Position = manager };
                User bob = new User { Name = "Bob", Company = google, Position = developer };
                User alice = new User { Name = "Alice", Company = microsoft, Position = developer };
                User kate = new User { Name = "Kate", Company = google, Position = manager };
                appContext.Users.AddRange(tom, bob, alice, kate);

                appContext.SaveChanges();

                var users = appContext.Users
                    .Include(c => c.Company)
                        .ThenInclude(co => co!.Country)
                            .ThenInclude(ca => ca!.Capital)
                    .Include(p => p.Position)
                    .ToList();

                foreach (var user in users)
                {
                    Console.WriteLine($"{user.Id}. {user.Name} - {user.Position?.Name}");
                    Console.WriteLine($"Company is: {user.Company!.Name}");
                    Console.WriteLine($"Country is: {user.Company.Country!.Name}");
                    Console.WriteLine($"Capital is: {user.Company.Country.Capital!.Name}");
                    Console.WriteLine("--------------------------------");
                }

                Console.WriteLine("UNIVERSITY");
                Course math = new Course { Name = "Math" };
                Course english = new Course { Name = "English" };
                Course literature = new Course { Name = "Literature" };
                Course geography = new Course { Name = "Geography" };
                Course history = new Course { Name = "History" };
                appContext.Courses.AddRange(literature, math, geography, history, english);

                Student mars = new Student { Name = "Valik" };
                Student don = new Student { Name = "Don" };
                Student diana = new Student { Name = "Diana", Courses = new List<Course> { literature, math } };
                Student vasya = new Student { Name = "Valik" };
                Student vika = new Student { Name = "Tom", Courses = new List<Course> { literature, math, geography, history, english } };
                Student nick = new Student { Name = "Bob" };
                Student alina = new Student { Name = "Valik", Courses = new List<Course>() { english, history } };
                Student jeny = new Student { Name = "Tom" };
                Student jack = new Student { Name = "Bob", Courses = new List<Course> { literature, math } };

                mars.Courses = new List<Course>() { english, math, geography };
                don.Courses = new List<Course>() { english, history };
                vasya.Courses = new List<Course>() { literature, math, history };
                nick.Courses = new List<Course>() { english, geography };
                appContext.Students.AddRange(mars, don, diana, vasya, vika, nick, alina, jeny, jack);

                var students = appContext.Students.Include(c => c.Courses);

                appContext.SaveChanges();

                foreach (Student student in students)
                {
                    Console.WriteLine($"Student: {student.Name} ");
                    foreach (Course course in student.Courses)
                    {
                        Console.WriteLine(course.Name);
                    }
                    Console.WriteLine("----------------------------");
                }
            }
        }
    }
}