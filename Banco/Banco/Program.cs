using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ficha_2
{
    internal class ID //Permite IDs únicos e números random sem repetições
    {
        private static Random rndGlobal = new Random(); 
        private static int idGlobal = 1000;
        public static int NewID() //gera novo ID, sempre maior que o último gerado
        {
            idGlobal += rndGlobal.Next(1, 30);
            return idGlobal;
        }
        public static int Rnd(int start, int end) //devolve int entre start e end, usando o Random global
        {
            return rndGlobal.Next(start, end);
        }
    }
    internal class Program
    {
        static List<Conta> Contas = new List<Conta>();
        //addAccounts e addMovement cria 10 contas aleatórias com um número aleatório de movimentos, até 12
        static void AddAccounts(int n)
        {
            double saldo = 0;
            int i = 0;
            for (i = 0; i < n; i++) //8 contas nao especiais aleatórias
            {
                bool especial = false;//30% contas especiais
                if (ID.Rnd(0, 10) > 8)
                    especial = true;
                saldo = ID.Rnd(1500, 5000);
                Contas.Add(new Conta(ID.NewID(), saldo, especial, ID.Rnd(0,1000)));
                Contas[i].Movimentacao.Add(new Movimento(saldo, false, "Abertura de conta " + DateTime.Now.ToString()));
                AddMovement(i, ID.Rnd(6, 16));
            }
            i--;
            //conta com saldo negativo //ultrapassa limiar!!!!!!!!!!!!!!!!!!!!!!
            double divida = Contas[i].saldo + ID.Rnd(50, 350);
            Contas[i].Movimentacao.Add(new Movimento(divida, true, "Pagamento " + DateTime.Now.ToString()));
            Contas[i].saldo -= divida;
        }
        static void AddMovement(int index, int howMany)
        {
            for(int i = 0;i < howMany; i++) //gera howMany vezes um movimento, ~80% levantamentos, ~20% depósitos
            {
                //valor,bool debito,descricao
                int opt = ID.Rnd(1, 10);
                if (opt < 9)
                {
                    string s = "";
                    if (ID.Rnd(1, 10) > 5)
                        s = "Levantamento ";
                    else
                        s = "Pagamento ";
                    double valor = ID.Rnd(5, 350);
                    if (Contas[index].saldo < valor)
                        break;
                    Contas[index].Movimentacao.Add(new Movimento(valor, true, s + DateTime.Now.ToString()));
                    Contas[index].saldo -= valor;
                }
                else
                {
                    double valor = ID.Rnd(650, 1000);
                    Contas[index].Movimentacao.Add(new Movimento(valor, false, "Depósito " + DateTime.Now.ToString()));
                    Contas[index].saldo += valor;
                }
            }
        }
        static bool HasAccount()//verifica se existem contas
        {
            if (Contas.Count() == 0)
                return false;
            return true;
        } 
        static void AguardarInput()//Permite pausar a consola até ser premido ENTER. Pode ser usada qualquer outra tecla
        {
            Console.WriteLine("\n ENTER para Continuar");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
        static void PrintBox(string title)//caixa informativa na consola
        {
            Console.WriteLine(" +---------------------------------------------------+");
            Console.WriteLine(" | {0}|", title.PadRight(50));
            Console.WriteLine(" +---------------------------------------------------+");
        }
        static bool VerifTemContas(int opcao)//Se nao existirem contas, bloqueia todas as opções exceto Criar Conta
        {
            //Contains verifica se o vetor que inserimos contém o parametro.
            //Ou seja, se a opcao selecionada nao for 6 (Criar contas) & nao existirem contas (hasAccount retorna falso), retorna falso
            if (new[] { 1, 2, 3, 4, 5, 7, 8 , 9}.Contains(opcao) && !HasAccount())
                return false;
            else
                return true;
        }
        static int ReadInt()//Lógica para ler e verificar um int
        {
            string s;
            do
            {
                Console.Write(" > ");
                s = Console.ReadLine();
            } while (!int.TryParse(s, out _) || s[0] == '-');
            int num = int.Parse(s);
            return num;
        }
        static int IntIntroduzir(string title)//Dá um titulo na consola que recebe por parametro e lê um int 
        {
            Console.WriteLine(" +---------------------------------------------------+");
            Console.WriteLine(" | {0}|", title.PadRight(50));
            Console.WriteLine(" +---------------------------------------------------+");
            return ReadInt();
        }
        static double DoubleIntroduzir(string title)//Dá um titulo na consola que recebe por parametro. Lógica para ler e verificar um double. 
        {
            Console.WriteLine(" +---------------------------------------------------+");
            Console.WriteLine(" | {0}|", title.PadRight(50));
            Console.WriteLine(" +---------------------------------------------------+");
            string s, sT;
            do
            {
                Console.Write/**/(" > ");
                s = Console.ReadLine();
                s = s.Replace(',', '.'); //Parse lê os pontos e não virgulas
            } while (!double.TryParse(s, out _) || s[0] == '-');
            double value = double.Parse(s);
            Console.WriteLine(value);
            return value;
        }
        static int VerifConta(int id)//verifica se id já existe. Retorna index da conta ou -1 se nao existir
        {
            for (int i = 0; i < Contas.Count(); i++)
            {
                if (Contas[i].id == id)
                    return i;
            }
            return -1;
        }
        static int LerConta(bool credito)//Lógica para print na consola do texto propriado, sendo debito ou crédito, lê e verifica uma conta introduzida e retorna o index da respetiva conta
            //retorna index da conta ou -1 quando cancelado pelo user
        {
            string s = "";
            if (credito)
                s = "creditada ";
            int id = -1;
            int index = -1;
            do
            {
                id = IntIntroduzir($" Conta {s}(0 para cancelar)");
                if (id == 0)
                    return -1; //user introduz 0 (voltar ao menu), retorna -1
                index = VerifConta(id); //index ou -1 se nao existir
                if (index < 0)
                    Console.WriteLine("\n Conta inválida");
            } while (index < 0); //Repete introdução de conta enquanto conta nao for encontrada
            return index;
        }
        static int VerifSaldo(int index, double value)//Verifica se conta possui saldo suficiente para operação
            //1 - tem saldo suficiente //2 - operação é permitida mas conta fica negativa (limiar ativa) //-1 - nao possui saldo suficiente
        {
            if (Contas[index].saldo >= value)
                return 1;
            else if (Contas[index].saldo + Contas[index].limiar >= value)
                return 2;
            else
            {
                return -1;
            }
        }
        static void MostrarContas()//Mostra os números de conta ativos
            //Sempre que uma opcao do menu necessita de um número de conta, é apresentado um resumo de contas ativas para ser mais fácil
        {
            PrintBox("Contas Ativas");
            int cnt = 0;
            foreach (Conta c in Contas)
            {
                if (cnt == 0)
                    Console.Write(" | ");
                Console.Write("{0}", c.id.ToString().PadRight(12));
                //Pad permite usar um número fixo de casas ocupadas, em que a string ocupa uma parte. 12 casas de espaço: a string ocupa 4, os outros 8 ficam com espaços
                if(cnt == 3) //4 números por linha apenas, ao 4º muda de linha e reinicia contador
                {
                    Console.Write("  |\n");
                    cnt = -1;
                }
                cnt++;
            }
            while (cnt < 4) //Se, na última linha, nao estiverem 4 números, este while preenche o resto da linha com espaços vazios, para formatar corretamente a caixa
            {
                Console.Write("{0}", "".PadRight(12));
                cnt++;
            }
            Console.Write("  |\n");
            Console.WriteLine(" +---------------------------------------------------+");
        }
        static void DesenharMenu()//Mostra menu
        {
            Console.Clear(); //limpa consola
            Console.WriteLine(" +---------------------------------------------------+");
            Console.WriteLine(" | Selecione uma opção:                              |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" | 1 | Levantar dinheiro                             |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" | 2 | Depositar dinheiro                            |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" | 3 | Realizar transferência                        |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" | 4 | Emitir saldo                                  |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" | 5 | Emitir extrato                                |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" | 6 | Criar conta                                   |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" | 7 | Alterar conta                                 |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" | 8 | Remover conta                                 |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" | 9 | Listagem de contas                            |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" | 0 | Sair                                          |");
            Console.WriteLine(" +---+-----------------------------------------------+");
        }
        static int LerOpcaoMenu()//em conjunto com o desenharMenu, lê uma opção escolhida sem ser necessário premir ENTER para aceitar
        {
            string key = "";
            do
            {
                key = key + Console.ReadKey(true).KeyChar; //lê a tecla premida
            } while (!int.TryParse(key, out _)); //verifica se é um número (opções do menu são números)
            int result = int.Parse(key);
            return result;
        }
        static bool DebitLogic(ref int index, ref double value)//Toda a lógica para efetuar um débito. Separado de levantar() pois é usado em transferir() também
        {
            index = LerConta(false); //user introduz uma conta. Se retorno for -1, o user cancelou a operação e volta ao menu
            if (index == -1)
                return false;
            value = DoubleIntroduzir(" Valor (0 para cancelar)"); //ler e verificar valor da operação
            if (value == 0)
                return false;//user cancelou operação
            //status: -1 sem saldo, 1 saldo suficiente, 2 suficiente mas entra no limiar
            int status = VerifSaldo(index, value);
            switch (status)
            {
                case -1:
                    Console.WriteLine("\n !!! Saldo insuficiente !!!");
                    AguardarInput();
                    return false;
                case 2:
                    Console.WriteLine("\n !!!!! ATENÇÃO !!!!!\n\n Prosseguir com a operação ativa o limiar da conta!\n\n 'sim' para prosseguir ou 'nao' para cancelar.");
                    string s;
                    do
                    {
                        Console.WriteLine(" 'sim' para prosseguir ou 'nao' para cancelar.");
                        Console.Write(" > ");
                        s = Console.ReadLine();
                    } while (s != "sim" && s != "nao"); //só aceita "sim" ou "nao"
                    if (s == "nao")
                        return false;//operação cancelada
                    break;
                default:
                    break;
            }
            return true;
        }
        static void Levantar()//Mostra resumo de contas, lê conta, lê valor e realiza debito se autorizado
        {
            MostrarContas();
            int index = -1; double value = -1;
            if (!DebitLogic(ref index, ref value))
                return; //user cancelou ou nao é permitido por saldo
            Contas[index].Movimentacao.Add(new Movimento(value, true, "Levantamento " + DateTime.Now.ToString())); //cria novo movimento, data e hora
            Contas[index].saldo -= value; //remove valor do saldo
            PrintBox($"Levantou {value.ToString("0.00")} € da conta {Contas[index].id}!"); //Mensagem de confirmação
            AguardarInput();
        }
        static void Depositar()//Mostra resumo de contas, lê conta, lê valor e realiza credito e autorizado
        {
            MostrarContas();
            int index = LerConta(true);
            if (index == -1)
                return; //user cancelou
            double value = DoubleIntroduzir(" Valor (0 para cancelar)");
            if (value == 0)
                return; //user cancelou
            Contas[index].Movimentacao.Add(new Movimento(value, false, "Deposito " + DateTime.Now.ToString())); //cria novo movimento, data e hora
            Contas[index].saldo += value; //adiciona valor ao saldo
            PrintBox($"Depositou {value.ToString("0.00")} € da conta {Contas[index].id}!"); //Mensagem de confirmação
            AguardarInput();
        }
        static void Transferir()//Mostra resumo de contas, lê conta a debitar, lê valor e verifica se debito é autorizado. Lê conta a creditar e realiza operação
        {
            MostrarContas();
            int indexSaida = -1; double value = -1;
            if (!DebitLogic(ref indexSaida, ref value))
                return;//user cancelou ou sem saldo
            //ler conta a creditar
            int indexEntrada = LerConta(true);
            if (indexEntrada == -1)
                return; //user cancelou
            //movimento de debito na primeira conta c/ mensagem
            Contas[indexSaida].Movimentacao.Add(new Movimento(value, true, $"Transferência PARA {Contas[indexSaida].id} " + DateTime.Now.ToString())); 
            Contas[indexSaida].saldo -= value;
            //movimento de credito na segunda conta c/ mensagem
            Contas[indexEntrada].Movimentacao.Add(new Movimento(value, false, $"Transferência DE {Contas[indexEntrada].id} " + DateTime.Now.ToString()));
            Contas[indexEntrada].saldo += value;
            PrintBox($"Transferiu {value.ToString("0.00")} € de {Contas[indexSaida].id} para {Contas[indexEntrada].id}!");//mensagem de confirmação
            AguardarInput();
        }
        static void EmitirSaldo()//mostra resumo de contas, lê conta e mostra saldo
        {
            MostrarContas();
            int index = LerConta(false);
            PrintBox($"Saldo: {Contas[index].saldo} €");
            AguardarInput();
        }
        static void EmitirExtrato()//mostra resumo de contas, lê conta e mostra extrato completo de movimentos
        {
            MostrarContas();
            int index = LerConta(false);
            if (index == -1)
                return;//user cancelou
            PrintBox("Extrato: ");
            foreach (Movimento mov in Contas[index].Movimentacao) //print de cada movimento, ver Movimento.cs, override ToString
            {
                Console.WriteLine(mov.ToString());
                Console.WriteLine(" +---------------------------------------------------+");
            }
            Console.WriteLine(" +---------------------------------------------------+");
            Console.WriteLine(" | {0} |",("Saldo: " + Contas[index].saldo.ToString("0.00")).PadLeft(49));
            Console.WriteLine(" +---------------------------------------------------+");
            AguardarInput();
        }
        static void CriarConta()//Criar nova conta, com ID gerado automaticamente
        {
            string s;
            do
            {
                PrintBox("Conta especial? (sim / nao)");
                Console.Write(" > ");
                s = Console.ReadLine();
            } while (s != "sim" && s != "nao");//aceita "sim" ou "nao" apenas
            bool specialAcc = false;
            if (s == "sim")
                specialAcc = true;
            double limiar = DoubleIntroduzir("Qual o limiar autorizado?");
            int nConta;
            do
            {
                nConta = ID.NewID();
            } while (VerifConta(nConta) != -1);//verifica se conta já existe. Redundante, visto que o id criado é sempre superior aos anteriores
            Conta conta = new Conta(nConta, 0, specialAcc, limiar); //método de criação de nova conta
            Contas.Add(conta); //adicionar À lista de contas
            PrintBox($"Conta nº {nConta} criada!");//mensagem confirmação
            AguardarInput();
        }
        static void ContaEspecialTroca(int index)//Mostra se conta selecionada é especial e se o user quer alterar para o contrário
        {
            string opt = "nao ";
            if (Contas[index].especial)//verifica se conta é especial
                opt = "";
            string s;
            do
            {
                PrintBox($"A conta atual {opt}é especial. Trocar? (sim/nao)");//se conta nao for especial, print "nao é especial" para o user
                Console.Write(" > ");
                s = Console.ReadLine();
            } while (s != "sim" && s != "nao");//aceite apenas "sim" ou "nao"
            bool especial = (Contas[index].especial) ? false : true;
            Contas[index].especial = especial; //op ternario. se "nao", nada muda. se "sim", altera estado para o oposto
            string estado = (especial) ? "Sim" : "nao";
            Contas[index].Movimentacao.Add(new Movimento("Alteração de estado especial para "+ estado));//movimento sem valor apenas para registo (ver Movimento.cs, construtor linha 33)
            PrintBox($"Alterou o estado especial para "+ estado +"!");//mensagem de confirmação
            AguardarInput();
        }
        static void AlterarConta()//menu de alteração de contas. Alterar estado especial da conta ou limiar autorizado
        {
            Console.Clear();
            Console.WriteLine(" +---------------------------------------------------+");
            Console.WriteLine(" | ALTERAR CONTA                                     |");
            Console.WriteLine(" +---------------------------------------------------+");
            MostrarContas();
            int index = LerConta(false);
            if (index == -1)
                return;
            Console.WriteLine(" +---------------------------------------------------+");
            Console.WriteLine(" | Selecione uma opção:                              |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" | 1 | Conta especial                                |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" | 2 | Limiar autorizado                             |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" +---+-----------------------------------------------+");
            Console.WriteLine(" | 0 | Sair                                          |");
            Console.WriteLine(" +---+-----------------------------------------------+");
            string key = "";
            do
            {
                key = key + Console.ReadKey(true).KeyChar;
            } while (key != "1" && key != "2" && key != "0");//Lê tecla premida. Só continua for 0, 1 ou 2
            int opcao = int.Parse(key);
            switch (opcao)
            {
                case 0: //user cancelou
                    return;
                case 1: //alterar estado especial
                    ContaEspecialTroca(index);
                    break;
                case 2://alterar limiar
                    {
                        PrintBox($"Limiar Atual: {Contas[index].limiar}€"); //print limiar atual
                        double valor = DoubleIntroduzir($"Introduza o novo limiar"); //lê e verificar valor novo
                        Contas[index].limiar = valor; //autoriza limiar novo
                        Contas[index].Movimentacao.Add(new Movimento("Alteração de limiar para " + valor.ToString("0.00") + " €!"));//movimento sem valor apenas para registo (ver Movimento.cs, construtor linha 33)
                        PrintBox($"Alterou o limiar da conta {Contas[index].id} para {valor.ToString("0.00")} €!");//mensagem de confirmaçãoaguardarInput();
                        AguardarInput();
                        break;
                    }
            }
        }
        static void RemoverConta()//remove conta introduzida pelo user se saldo for 0
        {
            MostrarContas();
            int index = LerConta(false);
            if (index == -1)
                return; // user cancelou
            if (Contas[index].saldo != 0) //saldo diferente de 0, cancela operação
            {
                PrintBox("Conta ainda tem saldo!");
                AguardarInput();
                return;
            }
            string s;
            do
            {
                PrintBox($"Tem a certeza que pertende eliminar esta conta? (sim/nao)");
                Console.Write(" > ");
                s = Console.ReadLine();
            } while (s != "sim" && s != "nao"); //confirmação
            if (s == "nao")
                return;
            Contas.RemoveAt(index); //remove de Contas
        }
        static void MostrarContasDet()//Resumo detalhado das contas ativas
        {
            PrintBox("Contas Ativas");
            Console.Write(" | ID\t");
            Console.Write("{0}", "Saldo".PadLeft(17));
            Console.Write("{0}", "Limiar".PadLeft(14));
            Console.Write("{0}  |\n", "Especial?".PadLeft(12));
            Console.WriteLine(" +---------------------------------------------------+");
            foreach (Conta c in Contas)
            {
                Console.Write($" | {c.id}\t");
                Console.Write("{0}", c.saldo.ToString("0.00").PadLeft(17));
                Console.Write("{0}", c.limiar.ToString("0.00").PadLeft(14));
                string s = "";
                if (c.especial)
                    s = "Sim";
                Console.Write("   {0}|\n", s.PadRight(11));
                Console.WriteLine(" +---------------------------------------------------+");
            }
        }
        static void Main(string[] args)//BANCO
        {
            AddAccounts(10);
            do
            {
                DesenharMenu();
                int opcao = LerOpcaoMenu();
                Console.Clear();
                if (!VerifTemContas(opcao))//verifica se existem contas para as opções pertinentes
                {
                    Console.WriteLine("");
                    Console.WriteLine(" Crie contas primeiro!");
                    AguardarInput();
                }
                else
                {
                    switch (opcao)
                    {
                        case 0:
                            return;
                        case 1:
                            Levantar();
                            break;
                        case 2:
                            Depositar();
                            break;
                        case 3:
                            Transferir();
                            break;
                        case 4:
                            EmitirSaldo();
                            break;
                        case 5:
                            EmitirExtrato();
                            break;
                        case 6:
                            CriarConta();
                            break;
                        case 7:
                            AlterarConta();
                            break;
                        case 8:
                            RemoverConta();
                            break;
                        case 9:
                            MostrarContasDet();
                            AguardarInput();
                            break;
                        default:
                            break;
                    }
                }
            } while (true);
        }
    }
}
