using System.Diagnostics;
using Labb3.Context;
using Labb3.Entities;
using Microsoft.EntityFrameworkCore;

namespace Labb3;

public class NewDataManager
{
    public void FetchAllStudents (string sortBy, bool sortOrder)
    {
        using (var context = new MyDbContext())
        {
            IQueryable<Student> query;  
            
            if (sortBy.ToLower() == "firstname")
            {
                query = sortOrder
                    ? context.Students.OrderBy(s => s.FirstName)
                    : context.Students.OrderByDescending(s => s.FirstName);
            }
            else if (sortBy.ToLower() == "lastname")
            {
                query = sortOrder
                    ? context.Students.OrderBy(s => s.LastName)
                    : context.Students.OrderByDescending(s => s.LastName);
            }
            else
            {
                query = default; 
                Console.WriteLine("Felaktig sortering vald!");
            }

            if (query != null)
            {
                int counter = 1;
                Console.WriteLine($"All Students:\n\n" +
                                  $"{"First Name".PadRight(15)} {"Last Name"}\n" +
                                  $"--------------------------");
                foreach (var student in query)
                {
                    Console.WriteLine($"{counter.ToString().PadRight(5)} {student.FirstName.PadRight(15)} {student.LastName}");
                    counter++;
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("There are no students in the table");
            }
        }
    }

    public void FetchAllStudentsOfClass(string grade)
    {
        int yearBorn = ReturnYearBorn(grade); 
        using (var context = new MyDbContext())
        {
            var studentsOfClass = context.Students
                .Include(s => s.Class)
                .Where(s => s.Class.YearBorn == yearBorn)
                .ToList();

            if (studentsOfClass.Count != 0)
            {
                Console.Clear();
                Console.WriteLine($"All students in {grade} grade (born in {yearBorn}):\n\n" +
                                  $"{"First Name".PadRight(15)} {"Last Name"}\n" +
                                  $"--------------------------");
                foreach (var student in studentsOfClass)
                {
                    Console.WriteLine($"{student.FirstName.PadRight(15)} {student.LastName} ");
                }
            }
            else
            {
                Console.WriteLine("There are no students in that grade");
            }
        }
    }

    public static int ReturnYearBorn(string grade)
    {
        int age = 0;

        switch (grade)
        {
            case "1st":
                age = 13; 
                break;
            case "2nd":
                age = 14; 
                break;
            case "3rd":
                age = 15;
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
        int adoptage = 0;
        //after new year, the students will become one year older. 
        if (DateTime.Now.Month <= 7)
        {
            adoptage = 1; 
        }
        int yearBorn = DateTime.Now.Year - age - adoptage;
        return yearBorn;
    }

    public void AddNewStaff()
    {
        bool runMenu = true;
        string firstName = "";
        string lastName = "";
        int roleID = 0;
        var roleName = "";

        while (runMenu)
        {
            Console.Clear();
            
            //Need to ensure we empty the name-strings, in case we enter the loop a second time
            firstName = "";
            lastName = "";
            
            Console.WriteLine("Please add the required data: ");

            while (firstName.Length <= 1)
            {
                Console.Write("\n1. First Name: ");
                firstName = Console.ReadLine();
                if (firstName.Length <= 1)
                {
                    Console.WriteLine("The name needs to have at least two letters");
                }
            }

            while (lastName.Length <= 1)
            {
                Console.Write("\n2. Last Name: ");
                lastName = Console.ReadLine();
                if (lastName.Length <= 1)
                {
                    Console.WriteLine("The name needs to have at least two letters");
                }
            }

            bool runRoleLoop = true;

            while (runRoleLoop)
            {

                using (var context = new MyDbContext())
                {
                    var staffRoles = context.StaffRoles;

                    if (staffRoles.ToList().Count != 0)
                    {
                        Console.WriteLine($"\n"+"ID".PadRight(7) +"Roles");
                        Console.WriteLine("---------------------");
                        foreach (var role in staffRoles)
                        {
                            Console.WriteLine($"[{role.Id}]".PadRight(7) + $"{role.StaffRoleName}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("There are no staffRoles in the table");
                    }
                    string answer = "";
                    while (!int.TryParse(answer, out roleID))
                    {
                        Console.Write("\nPlease enter the role-ID of the new staff: ");
                        answer = Console.ReadLine();
                    }

                    bool idExists = context.StaffRoles.Any(sr => sr.Id == roleID);
                    if (idExists)
                    {
                        var role = context.StaffRoles.FirstOrDefault(sr => sr.Id == roleID);
                        roleName = role.StaffRoleName; 
                        runRoleLoop = false; 
                    }
                    else
                    {
                        Console.WriteLine("You have enter a non-existing role-ID. Press enter to try again");
                        while (Console.ReadKey(true).Key != ConsoleKey.Enter) {};
                    }
                }
            }

            bool runCheckAnswer = true; 
            
            while (runCheckAnswer)
            {
                Console.Clear();
                
                Console.WriteLine($"You have entered the following: \n" +
                                  $"Name {firstName} {lastName} \n" +
                                  $"Role: {roleName}");
                Console.WriteLine("Is this correct? Y/N");
                
                ConsoleKeyInfo answer = Console.ReadKey();
               
                switch (answer.KeyChar)
                {
                    case 'y':
                        runCheckAnswer = false; 
                        runMenu = false;
                        break; 
                    case 'n':
                        runCheckAnswer = false;
                        Console.WriteLine("\nPress enter to try again");
                        while (Console.ReadKey(true).Key != ConsoleKey.Enter){};
                        break;
                    default:
                        Console.WriteLine("Wrong input! Press enter to try again");
                        while (Console.ReadKey(true).Key != ConsoleKey.Enter){};
                        break; 
                }
            }
        }

        using (var context = new MyDbContext())
        {
            //create a new staff
            var newStaff = new Staff()
            {
                FirstName = firstName,
                LastName = lastName,
                StaffRoleId = roleID
            }; 
            
            //adding the new staff to context and save
            context.Staff.Add(newStaff);
            context.SaveChanges();
            Console.WriteLine("\nThe new staff has been saved to the database");
        }
    }
}