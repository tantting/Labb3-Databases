namespace Labb3.Entities;
/// <summary>
/// Create a connection table for being able and create a strongly typed query when joining Students and Classes
/// </summary>
public class StudentClass
{
    public Student Student { get; set; }

    public Class Class { get; set; }
}