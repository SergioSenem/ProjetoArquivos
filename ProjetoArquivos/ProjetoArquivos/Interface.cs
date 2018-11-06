using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ProjetoArquivos
{
    public class Interface
    {
        private FileController fileController;

        public Interface()
        {
            fileController = new FileController();
        }

        public void Start()
        {
            ShowPrincipalMenu();
            ChooseOption();
        }

        public void ShowPrincipalMenu()
        {
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("1 - Criar arquivo (Comum)");
            Console.WriteLine("2 - Criar arquivo (Arvore B)");
            Console.WriteLine("3 - Ler arquivo (Comum)");
            Console.WriteLine("4 - Ler arquivo (Arvore B)");
        }

        public void ChooseOption()
        {
            int choice;
            if(!int.TryParse(Console.ReadLine(), out choice))
            {
                throw new Exception("Erro ao ler opção, favor digitar corretamente!");
            }

            try
            {
                switch (choice)
                {
                    case 1:
                        ShowCreateFile();
                        break;
                    case 2:
                        //CreateTreeFile();
                        break;
                    case 3:
                        ReadFile();
                        break;
                    case 4:
                        //ReadTreeFile();
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region CriacaoArquivo

        public void ShowCreateFile()
        {
            Console.WriteLine("Como deseja criar o arquivo?");
            Console.WriteLine("1 - Criar um arquivo com n registros");
            Console.WriteLine("2 - Criar um arquivo com um tamanho n");

            ChooseCreateFileMode();
        }

        public void ChooseCreateFileMode()
        {
            int choice;
            try
            {
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        CreateFileByRows();
                        break;
                    case 2:
                        ShowCreateFileBySize();
                        break;
                    default:
                        Console.WriteLine("Valor inválido!");
                        break;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void CreateFileByRows()
        {
            try
            {
                Console.Write("Digite o número de registros desejado: ");
                int rows = int.Parse(Console.ReadLine());

                fileController.WriteFileByRow(rows);

                Console.WriteLine("Arquivo criado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public void ShowCreateFileBySize()
        {
            Console.WriteLine("Selecione a unidade:");
            Console.WriteLine("1 - KB");
            Console.WriteLine("2 - MB");
            Console.WriteLine("3 - GB");

            ChooseCreateFileBySize();
        }

        public void ChooseCreateFileBySize()
        {
            try
            {
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        ShowCreateFileBySizeOption(FileSizeMode.Kilobyte);
                        break;
                    case 2:
                        ShowCreateFileBySizeOption(FileSizeMode.Megabyte);
                        break;
                    case 3:
                        ShowCreateFileBySizeOption(FileSizeMode.Gigabyte);
                        break;
                    default:
                        Console.WriteLine("Valor inválido!");
                        break;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void ShowCreateFileBySizeOption(FileSizeMode fileMode)
        {
            try
            {
                Console.Write("Digite o tamanho do arquivo: ");
                long size = int.Parse(Console.ReadLine());
                size = size * (long)fileMode;

                Stopwatch sw = Stopwatch.StartNew();
                fileController.WriteFileBySize(size);
                sw.Stop();

                Console.WriteLine("Arquivo criado com sucesso!");
                Console.WriteLine("Tempo para criação do arquivo: " + sw.Elapsed);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LeituraArquivo

        public void ReadFile()
        {
            try
            {
                fileController.ReadFile();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}