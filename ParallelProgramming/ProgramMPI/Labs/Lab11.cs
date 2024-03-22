using MPI;
using ProgramMPI.Interfaces;

namespace ProgramMPI.Labs
{
    public class Lab11 : ILab
    {
        public string Name { get; set; }
        public string[] _args { get; set; }

        public Lab11(string name, string[] args)
        {
            Name = name;
            _args = args;
        }

        public void ProccessStart(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;

                //Console.WriteLine("Введите количество нитей n:");
                //int n = int.Parse(Console.ReadLine() ?? "1");

                int n = 3;

                int numProcesses = comm.Size;
                int processRank = comm.Rank;
                int totalThreads = n * numProcesses;

                var options = new ParallelOptions { MaxDegreeOfParallelism = 1 }; // Ограничиваем степень параллелизма до 1

                Parallel.For(0, n, options, i =>
                {
                    Print(i, processRank, totalThreads);
                });
            }
        }

        public void Start()
        {
            Console.WriteLine(this);

            ProccessStart(_args);

            Console.WriteLine("\n");
        }

        public override string ToString() => $"{Name} started classes:";

        private void Print(int index, int processRank, int totalThreads) => Console.WriteLine($"I am {index} thread from {processRank} process. Number of hybrid threads = {totalThreads}.");
    }
}
