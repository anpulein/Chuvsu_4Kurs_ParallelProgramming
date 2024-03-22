using MPI;
using ProgramMPI.Interfaces;

namespace ProgramMPI.Labs
{
    public class Lab9 : ILab
    {
        public string Name { get; set; }
        private string[] _args { get; set; }
        private bool _isBCast { get; set; }

        public Lab9(string name, string[] args, bool isBCast = false)
        {
            Name = name;
            _args = args;
            _isBCast = isBCast;
        }

        public void ProccessStart(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                int root = 0;
                string inputString = null;

                if (comm.Rank == root)
                {
                    Console.WriteLine("Введите строку:");
                    inputString = Console.ReadLine();
                    // Отправка длины строки каждому процессу
                    for (int i = 1; i < comm.Size; i++)
                    {
                        comm.Send(inputString.Length, i, 0);
                    }
                }

                int stringLength = 0;
                if (comm.Rank != root)
                {
                    // Получение длины строки
                    stringLength = comm.Receive<int>(root, 0);
                }
                else
                {
                    stringLength = inputString.Length;
                }

                char[] charArray = new char[stringLength];
                if (comm.Rank == root)
                {
                    charArray = inputString.ToCharArray();
                    // Отправка строки каждому процессу
                    for (int i = 1; i < comm.Size; i++)
                    {
                        comm.Send(charArray, i, 0);
                    }
                }
                else
                {
                    // Получение строки
                    charArray = comm.Receive<char[]>(root, 0);
                }

                // Подсчет символов
                Dictionary<char, int> charCount = new Dictionary<char, int>();
                foreach (char c in charArray)
                {
                    if (charCount.ContainsKey(c))
                    {
                        charCount[c]++;
                    }
                    else
                    {
                        charCount.Add(c, 1);
                    }
                }

                // Вывод результатов
                foreach (var entry in charCount)
                {
                    Print(entry.Key, entry.Value);
                }

            }
        }

        private void ProccessStartBCast(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                int root = 0; // Процесс, осуществляющий ввод и начальную рассылку данных
                string inputString = null;

                if (comm.Rank == root)
                {
                    Console.WriteLine("Введите строку:");
                    inputString = Console.ReadLine(); // Ввод строки процессом 0
                }

                // Рассылка длины строки всем процессам
                int stringLength = inputString?.Length ?? 0;
                
                comm.Broadcast(ref stringLength, root);

                // Инициализация массива символов для приема строки
                char[] charArray = new char[stringLength];

                if (comm.Rank == root)
                {
                    charArray = inputString.ToCharArray();
                }

                // Рассылка самой строки всем процессам
                comm.Broadcast(ref charArray, root);

                // Подсчет символов
                Dictionary<char, int> charCount = new Dictionary<char, int>();
                foreach (char c in charArray)
                {
                    if (charCount.ContainsKey(c))
                    {
                        charCount[c]++;
                    }
                    else
                    {
                        charCount.Add(c, 1);
                    }
                }

                // Вывод результатов
                foreach (var entry in charCount)
                {
                    Print(entry.Key, entry.Value);
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

        private void Print(char key, int value) =>
            Console.WriteLine($"{key} = {value}");
    }
}
