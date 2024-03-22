using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;
using ProgramMPI.Interfaces;

namespace ProgramMPI.Labs
{
    public class Lab7 : ILab
    {
        public string Name { get; set; }
        public string[] _args { get; set; }

        public Lab7(string name, string[] args)
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

                // Определение номера процесса, которому нужно отправить сообщение
                int dest = (rank + 1) % size;
                // Определение номера процесса, от которого нужно принять сообщение
                int source = (rank - 1 + size) % size;

                // Отправка номера своего процесса следующему процессу в кольце
                comm.Send(rank, dest, 0);

                // Получение сообщения от предыдущего процесса в кольце
                int receivedRank = comm.Receive<int>(source, 0);

                Print(comm.Rank, receivedRank);
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
            Console.WriteLine($"[{rank + 1}]: received message: '{receivedMessage}'");
    }
}
