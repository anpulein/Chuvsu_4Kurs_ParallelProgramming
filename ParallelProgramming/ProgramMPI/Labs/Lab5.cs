using MPI;
using ProgramMPI.Interfaces;

namespace ProgramMPI.Labs
{
    public class Lab5 : ILab
    {
        public string Name { get; set; }
        public string[] _args { get; set; }

        public Lab5(string name, string[] args)
        {
            Name = name;
            _args = args;
        }

        public void ProccessStart(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;

                if (comm.Rank == 0) // Master процесс
                {
                    for (int i = 1; i < comm.Size; i++)
                    {
                        int message = comm.Receive<int>(i, 0);
                        Print(message);
                    }
                }
                else // Slave процессы
                {
                    comm.Send(comm.Rank, 0, 0); // Отправка номера своего процесса master процессу
                }
            }
        }

        public void Start()
        {
            ProccessStart(_args);

            Console.WriteLine("\n");
        }

        public override string ToString() => $"{Name} started classes:";

        private void Print(int receivedMessage) =>
            Console.WriteLine($"received message: '{receivedMessage}'");
    }
}
