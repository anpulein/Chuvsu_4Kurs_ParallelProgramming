using ProgramMP.Interfaces;

namespace ProgramMP.Labs;

public class Lab9 : ILab
{
    public string Name { get; set; }

    public Lab9(string name)
    {
        Name = name;
    }

    public void ThreadStart()
    {
        Parallel.Invoke(
            () => ParallelRegion(1),
            () => ParallelRegion(2),
            () => ParallelRegion(3)
        );
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


    private void ParallelRegion(int sectionNumber)
    {
        Console.WriteLine($"[{Task.CurrentId}]: came in section {sectionNumber}");
        
        Parallel.For(0, 1, i =>
        {
            Console.WriteLine($"[{Task.CurrentId}]: parallel region");
        });
    }
}