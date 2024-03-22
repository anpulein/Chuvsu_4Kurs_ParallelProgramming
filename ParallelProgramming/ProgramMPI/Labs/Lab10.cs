using System.Diagnostics;
using MPI;
using ProgramMPI.Interfaces;

namespace ProgramMPI.Labs
{
    public class Lab10 : ILab
    {
        public string Name { get; set; }
        private string[] _args { get; set; }
        private bool _isBCast { get; set; }
        private Stopwatch _stopwatch = new Stopwatch();

        public Lab10(string name, string[] args, bool isBCast = false)
        {
            Name = name;
            _args = args;
            _isBCast = isBCast;
        }

        public void ProccessStart(string[] args)
        {
            //Console.WriteLine("Введите целое число N: ");
            //if (!int.TryParse(Console.ReadLine(), out int N) && N <= 0)
            //{
            //    Console.WriteLine("Некорректный ввод. Убедитесь, что вводите положительное целое число.");
            //    return;
            //}

            using (new MPI.Environment(ref args))
            {
                _stopwatch.Start();

                Intracommunicator comm = Communicator.world;
                int n = 1000000, localN;
                double step, sum = 0.0, pi;

                step = 1.0 / n;
                localN = n / comm.Size;

                for (int i = comm.Rank * localN; i < (comm.Rank + 1) * localN; i++)
                {
                    double x = (i + 0.5) * step;
                    sum += 4.0 / (1.0 + x * x);
                }

                double localSum = sum * step;

                // Использование MPI_Reduce для редукции результатов
                double globalSum = comm.Reduce(localSum, Operation<double>.Add, 0);

                _stopwatch.Stop();

                if (comm.Rank == 0)
                {
                    pi = globalSum;
                    Print(pi, _stopwatch.Elapsed);
                }
            }
        }

        private void ProccessStartBCast(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                _stopwatch.Start();

                Intracommunicator comm = Communicator.world;
                int n = 1000000, localN;
                double step, localSum = 0.0, pi = 0.0;

                step = 1.0 / n;
                localN = n / comm.Size;

                // Вычисление локальной суммы для каждого процесса
                for (int i = comm.Rank * localN; i < (comm.Rank + 1) * localN; i++)
                {
                    double x = (i + 0.5) * step;
                    localSum += 4.0 / (1.0 + x * x);
                }
                localSum *= step;

                if (comm.Rank == 0)
                {
                    // Процесс с рангом 0 суммирует свою локальную сумму и получает значения от других процессов
                    pi = localSum;
                    for (int i = 1; i < comm.Size; i++)
                    {
                        double tempSum = comm.Receive<double>(i, 0);
                        pi += tempSum;
                    }

                    _stopwatch.Stop();

                    Print(pi, _stopwatch.Elapsed);
                }
                else
                {
                    // Отправка локальной суммы процессу с рангом 0
                    comm.Send(localSum, 0, 0);
                }
            }
        }

        public void Start()
        {
            Console.WriteLine(this);

            if (!_isBCast)
                ProccessStart(_args);
            else
                ProccessStartBCast(_args);

            Console.WriteLine("\n");
        }

        public override string ToString() => $"{Name} started classes:";

        private void Print(double pi, TimeSpan time) => Console.WriteLine($"Result PI = {pi}, Time = {time.Milliseconds} Мс");
    }
}
