using MPI;
using ProgramMPI.Interfaces;

namespace ProgramMPI.Labs
{
    public class Lab8 : ILab
    {
        public string Name { get; set; }
        public string[] _args { get; set; }

        public Lab8(string name, string[] args)
        {
            Name = name;
            _args = args;
        }

        public void ProccessStart(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                int rank = comm.Rank;
                int size = comm.Size;

                for (int i = 0; i < size; i++)
                {
                    if (i != rank)
                    {
                        // Отправка сообщения от текущего процесса к процессу i
                        comm.Send(rank, i, 0);
                    }
                }

                for (int i = 0; i < size - 1; i++)
                {
                    // Получение сообщения от любого процесса
                    int receivedRank = comm.Receive<int>(Communicator.anySource, 0);
                    Print(comm.Rank, receivedRank);
                }
            }
        }

        public void Start()
        {
            Console.WriteLine(this);

            ProccessStart(_args);

            Console.WriteLine("\n");
        }

        public override string ToString() => $"{Name} started classes:";

        private void Print(int rank, int receivedMessage) =>
            Console.WriteLine($"[{rank}]: receive message '{receivedMessage}' from {receivedMessage}");
    }
}
