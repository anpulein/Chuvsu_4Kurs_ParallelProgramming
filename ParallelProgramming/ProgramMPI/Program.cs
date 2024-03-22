// See https://aka.ms/new-console-template for more information

using MPI;
using ProgramMPI.Labs;

namespace ProgramMPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var lab1 = new Lab1("Lab1", args);
            lab1.Start();

            var lab2 = new Lab2("Lab2", args);
            lab2.Start();

            var lab3 = new Lab3("Lab3", args);
            lab3.Start();

            var lab4 = new Lab4("Lab4", args);
            lab4.Start();

            var lab5 = new Lab5("Lab5", args);
            lab5.Start();

            var lab6 = new Lab6("Lab6", args);
            lab6.Start();

            var lab7 = new Lab7("Lab7", args);
            lab7.Start();

            var lab8 = new Lab8("Lab8", args);
            lab8.Start();

            var lab9 = new Lab9("Lab9", args);
            lab9.Start();

            lab9 = new Lab9("Lab9", args, true);
            lab9.Start();

            var lab10 = new Lab10("Lab10", args);
            lab10.Start();

            lab10 = new Lab10("Lab10", args, true);
            lab10.Start();



            var lab11 = new Lab11("Lab11", args);
            lab11.Start();

            var lab12 = new Lab12("Lab12", args);
            lab12.Start();
        }
    }
}
