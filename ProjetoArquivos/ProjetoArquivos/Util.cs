using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoArquivos
{
    public enum FileSizeMode
    {
        Kilobyte = 1024,
        Megabyte = 1048576,
        Gigabyte = 1073741824
    }

    public class Constants
    {
        public static int FilePage = 50;
    }

    //Classe auxiliar para randomizar uma lista
    public static class Shuffler
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
