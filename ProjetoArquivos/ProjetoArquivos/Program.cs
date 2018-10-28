using System;

namespace ProjetoArquivos
{
    class Program
    {
        static void Main(string[] args)
        {
            FileController fc = new FileController();

            fc.WriteFile();
            //fc.ReadRow();

            Console.ReadKey();
        }
    }
}
