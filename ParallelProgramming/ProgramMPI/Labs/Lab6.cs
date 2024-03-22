using MPI;
using ProgramMPI.Interfaces;

namespace ProgramMPI.Labs
{
    public class Lab6 : ILab
    {
        public string Name { get; set; }
        public string[] _args { get; set; }

        public Lab6(string name, string[] args)
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
                    // Процесс 0 отправляет число 45 процессу 1
                    int message = 45;
                    // Неблокирующая отправка сообщения
                    Request sendRequest = comm.ImmediateSend(new int[] { message }, 1, 0);
                    sendRequest.Wait(); // Ожидаем завершения отправки
                }
                else if (comm.Rank == 1)
                {
                    // Процесс 1 получает сообщение от процесса 0
                    int[] receivedMessage = new int[1]; // Буфер для получения числа

                    // Неблокирующее получение сообщения
                    Request receiveRequest = comm.ImmediateReceive<int>(0, 0, receivedMessage);
                    receiveRequest.Wait(); // Ожидание получения сообщения

                    Print(receivedMessage[0]);
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
