using ProgramMP.Interfaces;

namespace ProgramMP.Labs;

public class Lab2 : ILab
{
    public string Name { get; set; }
    private readonly bool _isPos;

    public Lab2(string name, bool isPos = false)
    {
        Name = name;
        _isPos = isPos;
    }

    public void ThreadStart()
    {
        Console.WriteLine("Введите кол-во нитей: ");

        if (int.TryParse(Console.ReadLine(), out int threads) && threads > 0)
        {
            Parallel.For(0, threads, i =>
            {
                if (!_isPos || i % 2 == 0) 
                {
                    Print(i, threads);
                }
            });
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


    private void Print(int index, int threads) => Console.WriteLine($"I am {index} thread from {threads} threads!");
}