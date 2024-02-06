using ProgramMP.Interfaces;

namespace ProgramMP.Labs;

public class Lab11 : ILab
{
    public string Name { get; set; }
        
    public Lab11(string name)
    {
        Name = name;
    }
        
    public void Start()
    {
        Console.WriteLine(this);
            
        ThreadStart();
            
        Console.WriteLine("\n");
    }
        
    public void ThreadStart()
    {
        Console.WriteLine("Введите целое число N: ");
        if (!int.TryParse(Console.ReadLine(), out int N) && N <= 0)
        {
            Console.WriteLine("Некорректный ввод. Убедитесь, что вводите положительное целое число.");
            return;
        }
            
            
        var pi = GetNumberPi(N);
    
        Print(pi);
    }
    
    public override string ToString()
    {
        return $"{Name} started classes:";
    }
    
    
    private void Print(double pi) => Console.WriteLine($"Result PI = {pi}");
    
    private double GetNumberPi(int n)
    {
    
        double sum = 0.0;
        double step = 1.0 / n;
    
        object locker = new object();
            
        Parallel.For(0, n - 1, i =>
        {
            double x = (i + 0.5) * step;
            double term = 4.0 / (1.0 + x * x);
            double localSum = term * step; // Вычисление частичной суммы для итерации
                
            lock (locker)
            {
                sum += localSum;
            }
        });
    
        return sum;
    }
}