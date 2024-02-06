using ProgramMP.Interfaces;

namespace ProgramMP.Labs;

public class Lab10 : ILab
{
    public string Name { get; set; }
    private readonly object _lockObject = new object(); // Объект блокировки
    
    public Lab10(string name)
    {
        Name = name;
    }
    
    public void ThreadStart()
    {
        Console.WriteLine("Введите кол-во нитей K: ");
        if (!int.TryParse(Console.ReadLine(), out int K) && K <= 0)
        {
            Console.WriteLine("Некорректный ввод. Убедитесь, что вводите положительное целое число.");
            return;
        }
        
        Console.WriteLine("Введите число N: ");
        if (!int.TryParse(Console.ReadLine(), out int N) && N <= 0)
        {
            Console.WriteLine("Некорректный ввод. Убедитесь, что вводите положительное целое число.");
            return;
        }
        
        int[] partialSums = new int[K];
        Task[] tasks = new Task[K];
        
        Parallel.For(0, K, new ParallelOptions { MaxDegreeOfParallelism = K }, taskNum =>
        {
            
            var self = N > K ? N / K : K / N;
            
            // Определение диапазона для каждой нити
            int localTaskNum = taskNum; // Локальная переменная для каждой итерации
            int start = localTaskNum * (self) + 1;
            int end = (localTaskNum == K - 1) ? N : (localTaskNum + 1) * (self);

            // Вычисление частичной суммы для каждой нити
            for (int i = start; i <= end; i++)
            {
                lock (_lockObject)
                {
                    partialSums[taskNum] += i;
                }
            }

            Print(localTaskNum, partialSums[localTaskNum]);
        });

        int totalSum = 0;
        foreach (int sum in partialSums)
        {
            totalSum += sum;
        }

        Console.WriteLine($"Sum = {totalSum}"); // Вывод общей суммы
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


    private void Print(int index, int sum) => Console.WriteLine($"[{index}]: Sum = {sum}");
}