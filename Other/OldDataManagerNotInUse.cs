using Labb3.Context;
using Microsoft.Data.SqlClient; 
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Labb3.Entities;

public class OldDataManagerNotInUse
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sortBy"></param>
    /// <param name="sortOrder">Set true for ascending, false for descending</param>
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

    public void FetchAllStudentsOfClass(string sortBy, bool sortOrder, int yearBornInput)
    {
            using (var context = new MyDbContext())
            {
                bool continueMethod = true; 
                //IQueryable<Student> query = context.Students;
                IQueryable<Student> query = context.Students;
                    
                    var studentsOfClass = query
                        .Join(context.Classes,
                            s => s.ClassId,
                            c => c.Id,
                            (s, c) => new
                            {
                                studentFirstName = s.FirstName,
                                studentLastName = s.LastName,
                                classYearBorn = c.YearBorn
                            })
                        .Where(result => result.classYearBorn == yearBornInput)
                        .AsQueryable();
                    
                    //Testar att jag har fått ut rätt namn
                    /*
                    var result = query.ToList();
                    int counter = 1; 
                    foreach (var student in result)
                    {
                        Console.Write($"{counter.ToString().PadRight(5)} {student}");
                        Console.WriteLine();
                        counter++; 
                    }
                    */
                
                //Performed chosen sorting on the data (first name/last name and ascending or descending)
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
                
                //If there are no prior errors stopping the method and 
                if (continueMethod && query != null)
                {
                    int adoptage = 0;
                    //after new year, the students will become one year older. 
                    if (DateTime.Now.Month <= 7)
                    {
                        adoptage = 1; 
                    }
                    int age = DateTime.Now.Year - yearBornInput - adoptage;
                    
                    //grade as in 1st year of Highschool etc.
                    string grade = "";

                    switch (age)
                    {
                        case 13:
                            grade = "1st";
                            break;
                        case 14:
                            grade = "2nd";
                            break;
                        case 15:
                            grade = "3rd";
                            break;
                        default:
                            Console.WriteLine("The birth year is invalid");
                            break;
                    }
                    //Print result
                    int counter = 1; 
                    Console.WriteLine($"All Students in {grade} year:\n\n" +
                                      $"{"#".PadRight(5)} {"First Name".PadRight(15)} {"Last Name"}\n" +
                                      $"--------------------------");
                    foreach (var result in query)
                    {
                        //Use Information from Student combined with the joined result to fetch the data
                        //var student = result.Student;
                        Console.WriteLine($"{counter.ToString().PadRight(5)} {result.FirstName.PadRight(15)} {result.LastName}");
                        counter++; 
                    }
                }
                else
                {
                    Console.WriteLine("There are no students in the table");
                }
            }
    }
    
        public void FetchAllStudentsOfClass2(string sortBy, bool sortOrder, int yearBornInput)
    {
            using (var context = new MyDbContext())
            {
                bool continueMethod = true; 
                IQueryable<Student> query = context.Students;
                IQueryable<dynamic> queryIncludingClass = query;  

                bool yearExists = context.Classes
                    .Any(c => c.YearBorn == yearBornInput);

                if (yearExists)
                {
                    queryIncludingClass = query
                        .Join(context.Classes,
                            s => s.ClassId,
                            c => c.Id,
                            (s, c) => new
                            {
                                studentFirstName = s.FirstName,
                                studentLastName = s.LastName,
                                classYearBorn = c.YearBorn
                            })
                        .Where(result => result.classYearBorn == yearBornInput);

                    var result = queryIncludingClass.ToList(); 
                }
                else
                {
                    continueMethod = false;
                    Console.WriteLine("Den årgången finns ej på skolan!");
                }
                //Performed chosen sorting on the data (first name/last name and ascending or descending)
                if (continueMethod && sortBy.ToLower() == "firstname")
                {
                    queryIncludingClass = sortOrder
                        ? context.Students.OrderBy(s => s.FirstName)
                        : context.Students.OrderByDescending(s => s.FirstName);
                }
                else if (sortBy.ToLower() == "lastname")
                {
                    queryIncludingClass = sortOrder
                        ? context.Students.OrderBy(s => s.LastName)
                        : context.Students.OrderByDescending(s => s.FirstName);
                }
                else
                {
                    continueMethod = false; 
                    Console.WriteLine("Felaktig sortering vald!");
                }

                if (continueMethod && queryIncludingClass != null)
                {
                    int adoptgrade = 0;
                    //after new year, the students will become one year older. 
                    if (DateTime.Now.Month <= 7)
                    {
                        adoptgrade = 1; 
                    }
                    int age = DateTime.Now.Year - yearBornInput - adoptgrade;
                    
                    //grade as in 1st year of Highschool etc.
                    string grade = "";

                    switch (age)
                    {
                        case 13:
                            grade = "1st";
                            break;
                        case 14:
                            grade = "2nd";
                            break;
                        case 15:
                            grade = "3rd";
                            break;
                        default:
                            Console.WriteLine("The birth year is invalid");
                            break;
                    }

                    int counter = 1; 
                    Console.WriteLine($"All Students in {grade} year:\n\n" +
                                      $"{"#".PadRight(5)} {"First Name".PadRight(15)} {"Last Name"}\n" +
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
}