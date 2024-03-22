using MPI;
using ProgramMPI.Interfaces;

namespace ProgramMPI.Labs
{
    public class Lab4 : ILab
    {
        public string Name { get; set; }
        public string[] _args { get; set; }

        public Lab4(string name, string[] args)
        {
            Name = name;
            _args = args;
        }

        public void ProccessStart(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;

                int message;

                if (comm.Rank == 0)
                {
                    // Процесс 0 начинает эстафету, отправляя свой номер процессу 1
                    message = comm.Rank;
                    comm.Send(message, (comm.Rank + 1) % comm.Size, 0);
                }

                // Все процессы, кроме процесса 0, получают сообщение
                if (comm.Rank != 0)
                {
                    message = comm.Receive<int>(comm.Rank - 1, 0);
                    Print(comm.Rank, message);
                }
                else
                {
                    // Процесс 0 также ожидает получение сообщения от последнего процесса
                    message = comm.Receive<int>(comm.Size - 1, 0);
                    Print(comm.Rank, message);
                }

                // Каждый процесс, получив сообщение, отправляет его следующему, инкрементировав на 1
                // Процесс 0 уже отправил сообщение, поэтому его исключаем
                if (comm.Rank != 0)
                {
                    comm.Send(message + 1, (comm.Rank + 1) % comm.Size, 0);
                }
            }
        }

        public void Start()
        {
            ProccessStart(_args);

            Console.WriteLine("\n");
        }

        public override string ToString() => $"{Name} started classes:";

        private void Print(int rank, int receivedMessage) =>
            Console.WriteLine($"[{rank + 1}]: received message: '{receivedMessage}'");
    }
}
