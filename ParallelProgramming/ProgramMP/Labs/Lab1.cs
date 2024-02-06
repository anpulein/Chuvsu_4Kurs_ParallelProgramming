using ProgramMP.Interfaces;

namespace ProgramMP.Labs;

public class Lab1 : ILab
{
    public string Name { get; set; }

    public Lab1(string name)
    {
        Name = name;
    }

    public void ThreadStart()
    {
        Parallel.For(0, 4, i =>
        {
            Print();
        });
    }

    public void Start()
    {
        Console.WriteLine(this);
        ThreadStart();
        
        Console.WriteLine("\n");
    }

    public override string ToString()
    {
        return $"{Name} started classes:";
    }


    private void Print() => Console.WriteLine("Hello world!");
}