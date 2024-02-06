namespace ProgramMP.Interfaces;

public interface ILab
{
    string Name { get; set; }

    /// <summary>
    /// Задание лабораторной работы
    /// </summary>
    void ThreadStart();

    /// <summary>
    /// Запуск лабораторной работы (Thread)
    /// </summary>
    void Start();
}