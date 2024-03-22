using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramMPI.Interfaces
{
    internal interface ILab
    {
        /// <summary>
        /// Задание лабораторной работы
        /// </summary>
        void ProccessStart(string[] args);

        /// <summary>
        /// Запуск лабораторной работы (Proccess)
        /// </summary>
        void Start();
    }
}
