using ProgramMP.Interfaces;

namespace ProgramMP.Labs;

public class Lab8 : ILab
{
    public string Name { get; set; }
    private Random rand;
    
    public Lab8(string name)
    {
        Name = name;
        rand = new Random();
    }
    
    public void Start()
    {
        Console.WriteLine(this);
        
        ThreadStart();
        
        Console.WriteLine("\n");
    }
    
    public void ThreadStart()
    {
        Console.WriteLine("Введите размер матриц n x n: ");
        int n = int.Parse(Console.ReadLine() ?? "0");
        
        Console.WriteLine("Заполнение матрицы A:");
        int[,] A = ReadMatrix(n);

        Console.WriteLine("Заполнение матрицы B:");
        int[,] B = ReadMatrix(n);
        
        // Умножение матриц
        int[,] C = MultiplyMatricesParallel(A, B, n);

        // Вывод результата
        Console.WriteLine("Матрица C (A x B):");
        PrintMatrix(C, n);
    }

    public override string ToString()
    {
        return $"{Name} started classes:";
    }
    
    
    private int[,] ReadMatrix(int n)
    {
        var matrix = new int[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                // Console.Write($"Введите элемент [{i + 1}, {j + 1}]: ");
                // matrix[i, j] = int.Parse(Console.ReadLine() ?? "0"); // Примерные значения элементов матрицы
                matrix[i, j] = rand.Next(1, 15);
            }
        }
        return matrix;
    }
    
    private int[,] MultiplyMatricesParallel(int[,] A, int[,] B, int n)
    {
        var C = new int[n, n];
        
        int numProcs = 1;
        object locker = new object();

        Parallel.For(0, n, i =>
        {
            Console.WriteLine($"Count {numProcs++}");
            
            for (int j = 0; j < n; j++)
            {
                C[i, j] = 0;
                for (int k = 0; k < n; k++)
                {
                    C[i, j] += A[i, k] * B[k, j];
                }
            }
        });

        return C;
    }
    
    private void PrintMatrix(int[,] matrix, int n)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                // Console.Write($"{matrix[i, j]} ");
                // matrix[i, j] = rand.Next(1, 15);
            }
            // Console.WriteLine();
        }
    }
}