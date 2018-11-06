using System;

//1073741824 -> 1GB

namespace ProjetoArquivos
{
    class Program
    {
        static void Main(string[] args)
        {
            Interface io = new Interface();
            io.Start();

            Console.ReadKey();
        }
    }
}
