using MPI;
using ProgramMPI.Interfaces;

namespace ProgramMPI.Labs
{
    public class Lab2 : ILab
    {
        public string Name { get; set; }
        public string[] _args { get; set; }

        public Lab2(string name, string[] args)
        {
            Name = name;
            _args = args;
        }

        public void ProccessStart(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;

                if (comm.Rank == 0)
                {
                    Console.WriteLine(this);
                    Console.WriteLine($"{comm.Size} processes.");
                }

                Print(comm.Rank, comm.Size);
            }
        }

        public void Start()
        {
            ProccessStart(_args);

            Console.WriteLine("\n");
        }

        public override string ToString() => $"{Name} started classes:";

        private void Print(int rank, int size) =>
            Console.WriteLine($"I am {rank} process: {(rank % 2 != 0 ? "FIRST!" : "SECOND!")}");
    }
}
