using ProgramMP.Interfaces;

namespace ProgramMP.Labs;

public class Lab4 : ILab
{
    public string Name { get; set; }
    private readonly bool _isModif;

    public Lab4(string name, bool isModif = false)
    {
        Name = name;
        _isModif = isModif;
    }

    public void ThreadStart()
    {
        Console.WriteLine("Введите число N: ");

        if (int.TryParse(Console.ReadLine(), out int N) && N > 0)
        {
            int half = N / 2;
            int sum1 = 0, sum2 = 0;
            
            // Создание и запуск первой задачи для вычисления суммы от 1 до half
            var task1 = Task.Run(() =>
            {
                for (int i = 1; i <= half; i++)
                {
                    sum1 += i;
                }
                Print(0, sum1);
            });

            // Создание и запуск второй задачи для вычисления суммы от half+1 до N
            var task2 = Task.Run(() =>
            {
                for (int i = half + 1; i <= N; i++)
                {
                    sum2 += i;
                }
                Print(1, sum2);
            });

            Task.WaitAll(task1, task2); // Ожидание завершения обеих задач

            int totalSum = sum1 + sum2; // Общая сумма
            Console.WriteLine($"Общая сумма: {totalSum}");
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Убедитесь, что вводите положительное целое число.");
        }
        
    }

    private void ThreadStartModif()
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
        
        // Распределение работы по нитям
        for (int taskNum = 0; taskNum < K; taskNum++)
        {
            var self = N > K ? N / K : K / N;
            
            int localTaskNum = taskNum; // Локальная переменная для каждой итерации
            int start = localTaskNum * (self) + 1;
            int end = (localTaskNum == K - 1) ? N : (localTaskNum + 1) * (self);

            tasks[localTaskNum] = Task.Run(() =>
            {
                for (int i = start; i <= end; i++)
                {
                    partialSums[localTaskNum] += i;
                }

                Print(localTaskNum, partialSums[localTaskNum]);
            });
        }

        Task.WaitAll(tasks); // Ожидание завершения всех задач

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
        
        if (!_isModif)
            ThreadStart();
        else 
            ThreadStartModif();
        
        Console.WriteLine("\n");
    }

    public override string ToString()
    {
        return $"{Name} started classes:";
    }


    private void Print(int index, int sum) => Console.WriteLine($"[{index}]: Sum = {sum}");
}