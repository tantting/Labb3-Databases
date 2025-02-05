
using Labb3.Entities;
using Labb3.NonDataBaseClasses;
using Microsoft.Data.SqlClient;

namespace Labb3;

class Program
{
    static void Main(string[] args)
    {
        var menue = new Menue(); 
        
        menue.StartMenue();
    }
    
    //Connection_string for Scaffold if necessary: Server=localhost,1433;Database=HighSchoolDB; User = SA; Password = MyStrongPass123; Trust Server Certificate = true
}