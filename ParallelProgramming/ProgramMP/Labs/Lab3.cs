using ProgramMP.Interfaces;

namespace ProgramMP.Labs;

public class Lab3 : ILab
{
    public string Name { get; set; }

    public Lab3(string name)
    {
        Name = name;
    }

    public void ThreadStart()
    {
        Console.WriteLine("Введите кол-во нитей: ");

        if (int.TryParse(Console.ReadLine(), out int threads) && threads > 0)
        {
            Parallel.For(0, threads, Print);
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Убедитесь, что вводите положительное целое число.");
        }
        
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


    private void Print(int index) => Console.WriteLine($"I am {index} thread.");
}