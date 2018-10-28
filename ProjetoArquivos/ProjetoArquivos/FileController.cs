using System;
using System.Collections.Generic;
using System.IO;

namespace ProjetoArquivos
{
    //Classe que se encarregará de executar as funções de escrita e leitura de arquivos
    public class FileController
    {

        public int WriteFile()
        {
            BinaryWriter bw;

            try
            {
                if (Directory.Exists("C:/pra_docs"))
                {
                    bw = new BinaryWriter(new FileStream("C:/pra_docs/teste.dat", FileMode.Create));
                }
                else
                {
                    Directory.CreateDirectory("C:/pra_docs");
                    bw = new BinaryWriter(new FileStream("C:/pra_docs/teste.dat", FileMode.Create));
                }

            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message + "\nNão foi possível criar o arquivo!");
                return 0;
            }

            Venda venda = Venda.GenerateRandom();
            string row = Newtonsoft.Json.JsonConvert.SerializeObject(venda);
            bw.Write(row);

            bw.Close();

            bw = new BinaryWriter(new FileStream("C:/pra_docs/teste.dat", FileMode.Append));

            try
            {
                long fileSize = new System.IO.FileInfo("C:/pra_docs/teste.dat").Length;
                while (fileSize < 1073741824)
                {
                    venda = Venda.GenerateRandom();
                    row = Newtonsoft.Json.JsonConvert.SerializeObject(venda);
                    bw.Write(row);
                    fileSize = new System.IO.FileInfo("C:/pra_docs/teste.dat").Length;
                }
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message + "\nNão foi possível escrever no arquivo!");
                return 0;
            }

            bw.Close();
            return 1;
        }

        public void ReadRow()
        {
            BinaryReader br;

            try
            {
                if (Directory.Exists("C:/pra_docs"))
                {
                    br = new BinaryReader(new FileStream("C:/pra_docs/teste.dat", FileMode.Open));
                }
                else
                {
                    Directory.CreateDirectory("C:/pra_docs");
                    br = new BinaryReader(new FileStream("C:/pra_docs/teste.dat", FileMode.Open));
                }

                string row = br.ReadString();
                Venda venda = Newtonsoft.Json.JsonConvert.DeserializeObject<Venda>(row);

                Console.WriteLine(venda.cod_cliente + " / " + venda.cod_vendedor + " / " + venda.itens_comprados);

                br.Close();
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message + "/nErro ao ler arquivo!");
            }

        }

        #region Ordenacao

        //Método inicial para dividir o arquivo
        static void Split(string file)
        {
            int split_num = 1;
            StreamWriter sw = new StreamWriter(
              string.Format("c:\\split{0:d5}.dat", split_num));
            long read_line = 0;
            using (StreamReader sr = new StreamReader(file))
            {
                while (sr.Peek() >= 0)
                {
                    // Apenas para relatar o processo
                    if (++read_line % 5000 == 0)
                        Console.Write("{0:f2}%   \r",
                          100.0 * sr.BaseStream.Position / sr.BaseStream.Length);

                    // Escreve uma linha
                    sw.WriteLine(sr.ReadLine());

                    // Se o arquivo foi maior que 10MB e não for o fim dele, separe
                    if (sw.BaseStream.Length > 100000000 && sr.Peek() >= 0)
                    {
                        sw.Close();
                        split_num++;
                        sw = new StreamWriter(
                          string.Format("c:\\split{0:d5}.dat", split_num));
                    }
                }
            }
            sw.Close();
        }

        //Organiza um bloco (arquivo split)
        static void SortTheChunks()
        {
            foreach (string path in Directory.GetFiles("C:\\", "split*.dat"))
            {
                //Leitura e organização
                string[] contents = File.ReadAllLines(path);
                Array.Sort(contents);

                //Escrita em um arquivo separado
                string newpath = path.Replace("split", "sorted");
                File.WriteAllLines(newpath, contents);

                //Deleta arquivo split desorganizado
                File.Delete(path);

                //OPCIONAL, limpa a memória
                contents = null;
                GC.Collect();
            }
        }

        static void MergeTheChunks()
        {
            string[] paths = Directory.GetFiles("C:\\", "sorted*.dat"); //Vetor de blocos
            int chunks = paths.Length; // Número de blocos
            int recordsize = 100; // Média de tamanho da linha (objeto)
            int records = 10000000; // Total estimado de linhas
            int maxusage = 500000000; // Uso máximo de memória
            int buffersize = maxusage / chunks; // Bytes de cada fila
            double recordoverhead = 7.5; // O overhead de cada fila
            int bufferlen = (int)(buffersize / recordsize /
              recordoverhead); // Número de registros por fila

            // Abertura do arquivo
            StreamReader[] readers = new StreamReader[chunks];
            for (int i = 0; i < chunks; i++)
                readers[i] = new StreamReader(paths[i]);

            // Criação das filas
            Queue<string>[] queues = new Queue<string>[chunks];
            for (int i = 0; i < chunks; i++)
                queues[i] = new Queue<string>(bufferlen);

            // Carrega filas
            for (int i = 0; i < chunks; i++)
                LoadQueue(queues[i], readers[i], bufferlen);

            // Merge!
            StreamWriter sw = new StreamWriter("C:\\BigFileSorted.txt");
            bool done = false;
            int lowest_index, j, progress = 0;
            string lowest_value;
            while (!done)
            {
                // Apenas para reportar progresso
                if (++progress % 5000 == 0)
                    Console.Write("{0:f2}%   \r",
                      100.0 * progress / records);

                // Encontra bloco com o menor valor
                lowest_index = -1;
                lowest_value = "";
                for (j = 0; j < chunks; j++)
                {
                    if (queues[j] != null)
                    {
                        if (lowest_index < 0 ||
                          String.CompareOrdinal(
                            queues[j].Peek(), lowest_value) < 0)
                        {
                            lowest_index = j;
                            lowest_value = queues[j].Peek();
                        }
                    }
                }

                // Se não encontrou nada, saia
                if (lowest_index == -1) { done = true; break; }

                // Escrita no arquivo
                sw.WriteLine(lowest_value);

                // Remove registro da fila
                queues[lowest_index].Dequeue();
                // Se fila vazia, carregue mais registros
                if (queues[lowest_index].Count == 0)
                {
                    LoadQueue(queues[lowest_index],
                      readers[lowest_index], bufferlen);
                    // Se bloco chegou, esvazie fila
                    if (queues[lowest_index].Count == 0)
                    {
                        queues[lowest_index] = null;
                    }
                }
            }
            sw.Close();

            // Fechar e deletar arquivos temporários de blocos
            for (int i = 0; i < chunks; i++)
            {
                readers[i].Close();
                File.Delete(paths[i]);
            }
        }

        static void LoadQueue(Queue<string> queue, StreamReader file, int records)
        {
            for (int i = 0; i < records; i++)
            {
                if (file.Peek() < 0) break;
                queue.Enqueue(file.ReadLine());
            }
        }

        #endregion
    }
}
