using System;
using System.Collections.Generic;

namespace ProjetoArquivos
{
    //Classe base para representar uma venda (uma linha no arquivo)
    [Serializable]
    public class Venda
    {
        public int cod_cliente { get; set; }
        public int cod_vendedor { get; set; }
        public DateTime data_venda { get; set; }
        public float valor_venda { get; set; }
        public string itens_comprados { get; set; }

        public static Venda GenerateRandom()
        {

            Venda result = new Venda();

            List<string> nomes = ItensFactory.GenerateList();
            Random random = new Random();
            DateTime dateStart = new DateTime(1995, 1, 1);
            int dateRange = (DateTime.Today - dateStart).Days;
            int numItens = random.Next(nomes.Count);
            nomes.Shuffle();

            result.cod_cliente = random.Next(1, 1000);
            result.cod_vendedor = random.Next(1, 1000);
            result.data_venda = dateStart.AddDays(random.Next(dateRange));
            result.valor_venda = result.cod_cliente / result.cod_vendedor * 100;

            result.itens_comprados = "";

            for (int i = 0; i < (2 * numItens) / 3; i++)
            {
                result.itens_comprados += nomes[i];
            }

            return result;
        }

        public override string ToString()
        {
            string result = "\nCod Cliente: " + this.cod_cliente;
            result += "\nCod Vendedor: " + this.cod_vendedor;
            result += "\nData Venda: " + this.data_venda.ToString();
            result += "\nValor Venda: " + this.valor_venda;
            result += "\nItens: " + this.itens_comprados;
            result += "\n------------------------------";

            return result;
        }
    }

}
