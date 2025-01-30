using Labb3.Context;
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
}