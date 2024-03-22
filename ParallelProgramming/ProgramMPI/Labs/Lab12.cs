using MPI;
using ProgramMPI.Interfaces;

namespace ProgramMPI.Labs
{
    public class Lab12 : ILab
    {
        public string Name { get; set; }
        public string[] _args { get; set; }

        public Lab12(string name, string[] args)
        {
            Name = name;
            _args = args;
        }

        public void ProccessStart(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;

                int N;
                if (comm.Rank == 0)
                {
                    //Console.WriteLine("Введите целое число N: ");
                    //N = int.Parse(Console.ReadLine() ?? "1000000000"); // Значение по умолчанию, если ввод некорректен

                    N = 1000000000;
                }
                else
                {
                    N = 0;
                }

                // Рассылка значения N всем процессам
                comm.Broadcast(ref N, 0);

                double pi = CalculatePi(N, comm.Rank, comm.Size);

                // Сбор результатов вычисления 𝜋 со всех процессов
                double[] allPi = comm.Gather(pi, 0);

                if (comm.Rank == 0)
                {
                    double finalPi = 0;
                    foreach (var p in allPi)
                    {
                        finalPi += p;
                    }
                    Print(finalPi);
                }


            }
        }

        public void Start()
        {
            Console.WriteLine(this);

            ProccessStart(_args);

            Console.WriteLine("\n");
        }

        private double CalculatePi(int n, int rank, int size)
        {
            double sum = 0.0;
            double step = 1.0 / n;
            int localN = n / size;
            int start = rank * localN;
            int end = start + localN;

            object locker = new object();

            // Использование Parallel.For для имитации OpenMP
            Parallel.For(start, end, i =>
            {
                double x = (i + 0.5) * step;
                double term = 4.0 / (1.0 + x * x);

                lock (locker)
                {
                    sum += term;
                }

            });

            return sum * step;
        }

        public override string ToString() => $"{Name} started classes:";

        private void Print(double pi) => Console.WriteLine($"Result PI = {pi}");
    }
}
