using System.Collections.Concurrent;
using ProgramMP.Interfaces;

namespace ProgramMP.Labs;

public class Lab6 : ILab
{
    public string Name { get; set; }
    private const int K = 4; // Количество нитей
    private const int N = 10; // Количество итераций
    private const int chunkSize = 2; // Размер блока для static schedule
    
    public Lab6(string name)
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
        StaticSchedule();
        DynamicSchedule();
    }

    public override string ToString()
    {
        return $"{Name} started classes:";
    }


    private void StaticSchedule()
    {
        // Static Schedule
        Console.WriteLine("Static Schedule:");
        for (int i = 0; i < N; i += chunkSize)
        {
            Parallel.For(i, Math.Min(i + chunkSize, N), Print);
        }
    }
    
    private void DynamicSchedule()
    {
        // Создание списка задач
        var tasks = new Task[K];
        
        // Очередь для хранения номеров итераций
        var iterationsQueue = new ConcurrentQueue<int>();
        for (int i = 0; i < N; i++)
        {
            iterationsQueue.Enqueue(i);
        }
        
        Console.WriteLine("Dynamic Schedule:");
        for (int i = 0; i < K; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                while (iterationsQueue.TryDequeue(out int iteration))
                {
                    Print(iteration + 1);

                    // Имитация динамического распределения с chunk
                    for (int j = 1; j < chunkSize && iterationsQueue.TryDequeue(out int nextIteration); j++)
                    {
                        Print(nextIteration + 1);
                    }
                }
            });
        }

        Task.WaitAll(tasks); // Ожидание завершения всех задач
    }

    private void Print(int index) => Console.WriteLine($"[Thread {Task.CurrentId}]: calculation of the iteration number {index + 1}.");
}