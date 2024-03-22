using MPI;
using ProgramMPI.Interfaces;

namespace ProgramMPI.Labs
{
    public class Lab3 : ILab
    {
        public string Name { get; set; }
        public string[] _args { get; set; }

        public Lab3(string name, string[] args)
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
                    // Процесс с номером 0 отправляет сообщение процессу с номером 1
                    //string message = "Hello from process 0";
                    //byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    comm.Send(45, 1, 0); // Отправляем массив байтов процессу 1 с тегом 0
                }
                else if (comm.Rank == 1)
                {
                    // Процесс с номером 1 получает сообщение от процесса с номером 0
                    int receivedBytes = comm.Receive<int>(0, 0); // Получаем массив байтов от процесса 0 с тегом 0
                    //string receivedMessage = Encoding.UTF8.GetString(receivedBytes);
                    Print(receivedBytes); ;
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
            Console.WriteLine($"Received message: '{receivedMessage}'");
    }
}
