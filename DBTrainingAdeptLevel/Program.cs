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
                // Connections
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

                User tom = new() { Company = microsoft, Position = manager, 
                    UserProfile = new (){ 
                        Name = new () { Key = "Name", Value = "Tom"}, 
                        Age = new NameAndAge { Key = "Age", Value = "24" } 
                    } 
                };
                User bob = new() { Company = google, Position = developer, UserProfile = new()
                {
                    Name = new() { Key = "Name", Value = "Bob" },
                    Age = new NameAndAge { Key = "Age", Value = "49" }
                }
                };
                User alice = new() { Company = microsoft, Position = developer,
                    UserProfile = new()
                    {
                        Name = new() { Key = "Name", Value = "Alice" },
                        Age = new NameAndAge { Key = "Age", Value = "32" }
                    }
                };
                User kate = new() { Company = google, Position = manager,
                    UserProfile = new()
                    {
                        Name = new() { Key = "Name", Value = "Kate" },
                        Age = new NameAndAge { Key = "Age", Value = "19" }
                    }
                };
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
                    Console.WriteLine($"{user.Id}. {user.UserProfile.Name?.Value}, {user.UserProfile.Age?.Value} - {user.Position?.Name}");
                    Console.WriteLine($"Company is: {user.Company!.Name}");
                    Console.WriteLine($"Country is: {user.Company.Country!.Name}");
                    Console.WriteLine($"Capital is: {user.Company.Country.Capital!.Name}");
                    Console.WriteLine("--------------------------------");
                }

                // Many to many
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
                // Hierarchy

                MenuItem file = new MenuItem { Title = "File" };
                MenuItem edit = new MenuItem { Title = "Edit" };
                MenuItem open = new MenuItem { Title = "Open", Parent = file };
                MenuItem save = new MenuItem { Title = "Save", Parent = file };

                MenuItem copy = new MenuItem { Title = "Copy", Parent = edit };
                MenuItem paste = new MenuItem { Title = "Paste", Parent = edit };

                appContext.MenuItems.AddRange(file, edit, open, save, copy, paste);
                appContext.SaveChanges();

                var menuItems = appContext.MenuItems.ToList();
                Console.WriteLine("All Menu:");
                foreach (MenuItem m in menuItems)
                {
                    Console.WriteLine(m.Title);
                }
                Console.WriteLine();               
                var fileMenu = appContext.MenuItems.FirstOrDefault(m => m.Title == "File");
                if (fileMenu != null)
                {
                    Console.WriteLine(fileMenu.Title);
                    foreach (var m in fileMenu.Childrens)
                    {
                        Console.WriteLine($"---{m.Title}");
                    }
                }
            }
        }
    }
}
