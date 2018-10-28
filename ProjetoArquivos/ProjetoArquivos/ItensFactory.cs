using System.Collections.Generic;

namespace ProjetoArquivos
{
    //Gerador de itens
    public static class ItensFactory
    {

        public static List<string> GenerateList()
        {

            List<string> List = new List<string>();

            List.Add("Barbeador");
            List.Add("Computador");
            List.Add("Celular");
            List.Add("Televisão");
            List.Add("Geladeira");
            List.Add("Video-game");
            List.Add("Relógio");
            List.Add("Anel");
            List.Add("Sapatos");
            List.Add("Camisa");
            List.Add("Livro");
            List.Add("Estante");

            return List;
        }
    }
}
