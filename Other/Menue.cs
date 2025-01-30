using System.Diagnostics;
using System.Threading.Channels;

namespace Labb3.NonDataBaseClasses;

public class Menue
{
    private NewDataManager dataManager = new NewDataManager();

    public void StartMenue()
    {
        bool runMenu = true;

        while (runMenu)
        {
            Console.Clear();
            
            Console.WriteLine("Welcome to Monster High! \n" +
                              "\n" +
                              "What would you like to do today?\n\n" +
                              "[1] Fetch info of all students\n" +
                              "[2] Fetch all students of a specific class\n" +
                              "[3] Add new staff\n" +
                              "[4] End program");

            string choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Fetch all students");
                    Console.WriteLine("------------------");
                    string sortBy = SortBy();
                    bool sortrByAscending = SortOrder();
                    dataManager.FetchAllStudents(sortBy, sortrByAscending);
                    break;
                case "2":
                    dataManager.FetchAllStudentsOfClass(AskForGrade());
                    break;
                case "3":
                    dataManager.AddNewStaff();
                    break; 
                case "4":
                    runMenu = false;
                    Console.ReadKey();
                    Console.WriteLine("Have a nice day!");
                    break;
                default:
                    Console.WriteLine("Wrong input! Try again!");
                    Console.ReadKey(); 
                    break;
            }

            Console.WriteLine("\nPress enter to continue");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { };
            Console.ReadKey(); 
        }
    }

    public string SortBy()
    {
        string sortBy = ""; 

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Would you like to sort you list on:\n" +
                              "\n" +
                              "[1] First name" +
                              "\n" +
                              "or...\n" +
                              "[2] Last name");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    sortBy = "firstname";
                    return sortBy;
                case "2":
                    sortBy = "lastname";
                    return sortBy;
                default:
                    Console.WriteLine("Wrong input! Please, press enter and try again.");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { };
                    break;
            }
        }
    }
    
    public bool SortOrder()
    {
        bool sortByAscending = true; 

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Would you like to sort by order of\n" +
                              "\n" +
                              "[1] Ascending" +
                              "\n" +
                              "or...\n" +
                              "[2] Descending");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    sortByAscending = true;
                    return sortByAscending; 
                case "2":
                    sortByAscending = false;
                    return sortByAscending;
                default:
                    Console.WriteLine("Wrong input! Please, press enter and try again.");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { };
                    break;
            }
        }
    }

    public string AskForGrade()
    {
        while (true)
        {
            Console.Clear();
            
            Console.WriteLine("From what classs would you like to fetch the student data?\n" +
                              "\n" +
                              $"[1] 1st grade (born in {NewDataManager.ReturnYearBorn("1st")})\n" +
                              $"[2] 2nd grade (born in{NewDataManager.ReturnYearBorn("2nd")})\n" +
                              $"[3] 3rd grade (born in{NewDataManager.ReturnYearBorn("3rd")})\n");
            
            ConsoleKeyInfo choice = Console.ReadKey();

            switch (choice.KeyChar)
            {
                case '1':
                    return "1st"; 
                case '2':
                    return "2nd"; 
                case '3':
                    return "3rd"; 
                default:
                    Console.WriteLine("\n\nWrong input! Please, press enter and try again.");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { };
                    break;
            }
        }
    }
}