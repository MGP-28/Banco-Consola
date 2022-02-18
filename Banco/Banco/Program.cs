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
        public static int newID() //gera novo ID, sempre maior que o último gerado
        {
            idGlobal += rndGlobal.Next(1, 30);
            return idGlobal;
        }
        public static int rnd(int start, int end) //devolve int entre start e end, usando o Random global
        {
            return rndGlobal.Next(start, end);
        }
    }
    internal class Program
    {
        static List<Conta> Contas = new List<Conta>();
        //addAccounts e addMovement cria 10 contas aleatórias com um número aleatório de movimentos, até 12
        static void addAccounts(int n)
        {
            for (int i = 0; i < n - 2; i++) //8 contas não especiais aleatórias
            {
                Contas.Add(new Conta(ID.newID(), 0, false, ID.rnd(0,1000)));
                double saldo = ID.rnd(0, 1500);
                Contas[i].saldo = saldo;
                Contas[i].Movimentacao.Add(new Movimento(saldo, false, "Saldo inicial"));
                addMovement(i, ID.rnd(2, 12));
            }
            //conta especial
            Contas.Add(new Conta(ID.newID(), 0, true, ID.rnd(0, 1000)));
            Contas[8].saldo = ID.rnd(0, 1500);
            addMovement(8, ID.rnd(2, 12));
            //conta com saldo negativo
            Contas.Add(new Conta(ID.newID(), 0, false, ID.rnd(0, 1000)));
            Contas[9].saldo = ID.rnd(-250, -50);
            addMovement(9, ID.rnd(2, 12));
        }
        static void addMovement(int index, int howMany)
        {
            for(int i = 0;i < howMany; i++) //gera howMany vezes um movimento, ~80% levantamentos, ~20% depósitos
            {
                Movimento mov = new Movimento();//valor,bool debito,descricao
                int opt = ID.rnd(1, 10);
                if(opt < 9)
                    Contas[index].Movimentacao.Add(new Movimento(ID.rnd(15, 500), true, "Levantamento " + DateTime.Now.ToString())); //levantamento entre 15 e 500
                else
                    Contas[index].Movimentacao.Add(new Movimento(ID.rnd(200, 1000), false, "Depósito " + DateTime.Now.ToString())); //deposito entre 200 e 1000
            }
        }
        static bool hasAccount()//verifica se existem contas
        {
            if (Contas.Count() == 0)
                return false;
            return true;
        } 
        static void aguardarInput()//Permite pausar a consola até ser premido ENTER. Pode ser usada qualquer outra tecla
        {
            Console.WriteLine("\n ENTER para Continuar");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        } 
        static void desenharMenu()//Mostra menu
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
        static int lerOpcaoMenu()//em conjunto com o desenharMenu, lê uma opção escolhida sem ser necessário premir ENTER para aceitar
        {
            string key = "";
            do
            {
                key = key + Console.ReadKey(true).KeyChar; //lê a tecla premida
            } while (!int.TryParse(key, out _)); //verifica se é um número (opções do menu são números)
            int result = int.Parse(key);
            return result;
        }
        static bool verifTemContas(int opcao)//Se não existirem contas, bloqueia todas as opções exceto Criar Conta
        {
            //Contains verifica se o vetor que inserimos contém o parametro.
            //Ou seja, se a opcao selecionada não for 6 (Criar contas) & não existirem contas (hasAccount retorna falso), retorna falso
            if (new[] { 1, 2, 3, 4, 5, 7, 8 , 9}.Contains(opcao) && !hasAccount())
                return false;
            else
                return true;
        }
        static int readInt()//Lógica para ler e verificar um int
        {
            string s;
            do
            {
                Console.Write(" + ");
                s = Console.ReadLine();
            } while (!int.TryParse(s, out _) || s[0] == '-');
            int num = int.Parse(s);
            return num;
        }
        static int intIntroduzir(string title)//Dá um titulo na consola que recebe por parametro e lê um int 
        {
            Console.WriteLine(" +---------------------------------------------------+");
            Console.WriteLine(" | {0}|", title.PadRight(50));
            Console.WriteLine(" +---------------------------------------------------+");
            return readInt();
        }
        static double doubleIntroduzir(string title)//Dá um titulo na consola que recebe por parametro. Lógica para ler e verificar um double. 
        {
            Console.WriteLine(" +---------------------------------------------------+");
            Console.WriteLine(" | {0}|", title.PadRight(50));
            Console.WriteLine(" +---------------------------------------------------+");
            string s;
            do
            {
                Console.Write/**/(" + ");
                s = Console.ReadLine();
                s = s.Replace('.', ',');
            } while (!double.TryParse(s, out _) || s[0] == '-');
            double value = double.Parse(s);
            return value;
        }
        static int verifConta(int id)//verifica se id já existe. Retorna index da conta ou -1 se não existir
        {
            for (int i = 0; i < Contas.Count(); i++)
            {
                if (Contas[i].id == id)
                    return i;
            }
            return -1;
        }
        static int lerConta(bool credito)//Lógica para print na consola do texto propriado, sendo debito ou crédito, lê e verifica uma conta introduzida e retorna o index da respetiva conta
            //retorna index da conta ou -1 quando cancelado pelo user
        {
            string s = "";
            if (credito)
                s = "creditada ";
            int id = -1;
            int index = -1;
            do
            {
                id = intIntroduzir($" Conta {s}(0 para cancelar)");
                if (id == 0)
                    return -1; //user introduz 0 (voltar ao menu), retorna -1
                index = verifConta(id); //index ou -1 se não existir
                if (index < 0)
                    Console.WriteLine("\n Conta inválida");
            } while (index < 0); //Repete introdução de conta enquanto conta não for encontrada
            return index;
        }
        static int verifSaldo(int index, double value)//Verifica se conta possui saldo suficiente para operação
            //1 - tem saldo suficiente //2 - operação é permitida mas conta fica negativa (limiar ativa) //-1 - não possui saldo suficiente
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
        static void mostrarContas()//Mostra os números de conta ativos
            //Sempre que uma opcao do menu necessita de um número de conta, é apresentado um resumo de contas ativas para ser mais fácil
        {
            printBox("Contas Ativas");
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
            while (cnt < 4) //Se, na última linha, não estiverem 4 números, este while preenche o resto da linha com espaços vazios, para formatar corretamente a caixa
            {
                Console.Write("{0}", "".PadRight(12));
                cnt++;
            }
            Console.Write("  |\n");
            Console.WriteLine(" +---------------------------------------------------+");
        }
        static bool debitLogic(ref int index, ref double value)//Toda a lógica para efetuar um débito. Separado de levantar() pois é usado em transferir() também
        {
            index = lerConta(false); //user introduz uma conta. Se retorno for -1, o user cancelou a operação e volta ao menu
            if (index == -1)
                return false;
            value = doubleIntroduzir(" Valor (0 para cancelar)"); //ler e verificar valor da operação
            if (value == 0)
                return false;//user cancelou operação
            //status: -1 sem saldo, 1 saldo suficiente, 2 suficiente mas entra no limiar
            int status = verifSaldo(index, value);
            switch (status)
            {
                case -1:
                    Console.WriteLine("\n !!! Saldo insuficiente !!!");
                    aguardarInput();
                    return false;
                case 2:
                    Console.WriteLine("\n !!!!! ATENÇÃO !!!!!\n\n Prosseguir com a operação ativa o limiar da conta!\n\n 'sim' para prosseguir ou 'não' para cancelar.");
                    string s;
                    do
                    {
                        Console.WriteLine(" 'sim' para prosseguir ou 'não' para cancelar.");
                        Console.Write(" + ");
                        s = Console.ReadLine();
                    } while (s != "sim" && s != "não"); //só aceita "sim" ou "não"
                    if (s == "não")
                        return false;//operação cancelada
                    break;
                default:
                    break;
            }
            return true;
        }
        static void levantar()//Mostra resumo de contas, lê conta, lê valor e realiza debito se autorizado
        {
            mostrarContas();
            int index = -1; double value = -1;
            if (!debitLogic(ref index, ref value))
                return; //user cancelou ou não é permitido por saldo
            Contas[index].Movimentacao.Add(new Movimento(value, true, "Levantamento " + DateTime.Now.ToString())); //cria novo movimento, data e hora
            Contas[index].saldo -= value; //remove valor do saldo
            printBox($"Levantou {value.ToString("0.00")} € da conta {Contas[index].id}!"); //Mensagem de confirmação
            aguardarInput();
        }
        static void depositar()//Mostra resumo de contas, lê conta, lê valor e realiza credito e autorizado
        {
            mostrarContas();
            int index = lerConta(true);
            if (index == -1)
                return; //user cancelou
            double value = doubleIntroduzir(" Valor (0 para cancelar)");
            if (value == 0)
                return; //user cancelou
            Contas[index].Movimentacao.Add(new Movimento(value, false, "Deposito " + DateTime.Now.ToString())); //cria novo movimento, data e hora
            Contas[index].saldo += value; //adiciona valor ao saldo
            printBox($"Depositou {value.ToString("0.00")} € da conta {Contas[index].id}!"); //Mensagem de confirmação
            aguardarInput();
        }
        static void transferir()//Mostra resumo de contas, lê conta a debitar, lê valor e verifica se debito é autorizado. Lê conta a creditar e realiza operação
        {
            mostrarContas();
            int indexSaida = -1; double value = -1;
            if (!debitLogic(ref indexSaida, ref value))
                return;//user cancelou ou sem saldo
            //ler conta a creditar
            int indexEntrada = lerConta(true);
            if (indexEntrada == -1)
                return; //user cancelou
            //movimento de debito na primeira conta c/ mensagem
            Contas[indexSaida].Movimentacao.Add(new Movimento(value, true, $"Transferência PARA {Contas[indexSaida].id} " + DateTime.Now.ToString())); 
            Contas[indexSaida].saldo -= value;
            //movimento de credito na segunda conta c/ mensagem
            Contas[indexEntrada].Movimentacao.Add(new Movimento(value, false, $"Transferência DE {Contas[indexEntrada].id} " + DateTime.Now.ToString()));
            Contas[indexEntrada].saldo += value;
            printBox($"Transferiu {value.ToString("0.00")} € de {Contas[indexSaida].id} para {Contas[indexEntrada].id}!");//mensagem de confirmação
            aguardarInput();
        }
        static void printBox(string title)//caixa informativa na consola
        {
            Console.WriteLine(" +---------------------------------------------------+");
            Console.WriteLine(" | {0}|", title.PadRight(50));
            Console.WriteLine(" +---------------------------------------------------+");
        }
        static void emitirSaldo()//mostra resumo de contas, lê conta e mostra saldo
        {
            mostrarContas();
            int index = lerConta(false);
            printBox($"Saldo: {Contas[index].saldo} €");
            aguardarInput();
        }
        static void emitirExtrato()//mostra resumo de contas, lê conta e mostra extrato completo de movimentos
        {
            mostrarContas();
            int index = lerConta(false);
            if (index == -1)
                return;//user cancelou
            printBox("Extrato: ");
            foreach (Movimento mov in Contas[index].Movimentacao) //print de cada movimento, ver Movimento.cs, override ToString
            {
                Console.WriteLine(mov.ToString());
                Console.WriteLine(" +---------------------------------------------------+");
            }
            aguardarInput();
        }
        static void criarConta()//Criar nova conta, com ID gerado automaticamente
        {
            string s;
            do
            {
                printBox("Conta especial? (sim / não)");
                Console.Write(" + ");
                s = Console.ReadLine();
            } while (s != "sim" && s != "não");//aceita "sim" ou "não" apenas
            bool specialAcc = false;
            if (s == "sim")
                specialAcc = true;
            double limiar = doubleIntroduzir("Qual o limiar autorizado?");
            int nConta;
            do
            {
                nConta = ID.newID();
            } while (verifConta(nConta) != -1);//verifica se conta já existe. Redundante, visto que o id criado é sempre superior aos anteriores
            Conta conta = new Conta(nConta, 0, specialAcc, limiar); //método de criação de nova conta
            Contas.Add(conta); //adicionar À lista de contas
            printBox($"Conta nº {nConta} criada!");//mensagem confirmação
            aguardarInput();
        }
        static void contaEspecialTroca(int index)//Mostra se conta selecionada é especial e se o user quer alterar para o contrário
        {
            string opt = "não ";
            if (Contas[index].especial)//verifica se conta é especial
                opt = "";
            string s;
            do
            {
                printBox($"A conta atual {opt}é especial. Trocar? (sim/não)");//se conta não for especial, print "não é especial" para o user
                Console.Write(" + ");
                s = Console.ReadLine();
            } while (s != "sim" && s != "não");//aceite apenas "sim" ou "não"
            bool especial = (Contas[index].especial) ? false : true;
            Contas[index].especial = especial; //op ternario. se "não", nada muda. se "sim", altera estado para o oposto
            string estado = (especial) ? "Não" : "Sim";
            Contas[index].Movimentacao.Add(new Movimento("Alteração de estado especial para "+ estado));//movimento sem valor apenas para registo (ver Movimento.cs, construtor linha 33)
            printBox($"Alterou o estado especial para "+ estado +"!");//mensagem de confirmação
            aguardarInput();
        }
        static void alterarConta()//menu de alteração de contas. Alterar estado especial da conta ou limiar autorizado
        {
            Console.Clear();
            Console.WriteLine(" +---------------------------------------------------+");
            Console.WriteLine(" | ALTERAR CONTA                                     |");
            Console.WriteLine(" +---------------------------------------------------+");
            mostrarContas();
            int index = lerConta(false);
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
                    contaEspecialTroca(index);
                    break;
                case 2://alterar limiar
                    {
                        printBox($"Limiar Atual: {Contas[index].limiar}€"); //print limiar atual
                        double valor = doubleIntroduzir($"Introduza o novo limiar"); //lê e verificar valor novo
                        Contas[index].limiar = valor; //autoriza limiar novo
                        Contas[index].Movimentacao.Add(new Movimento("Alteração de limiar para " + valor.ToString("0.00") + " €!"));//movimento sem valor apenas para registo (ver Movimento.cs, construtor linha 33)
                        printBox($"Alterou o limiar da conta {Contas[index].id} para {valor.ToString("0.00")} €!");//mensagem de confirmaçãoaguardarInput();
                        aguardarInput();
                        break;
                    }
            }
        }
        static void removerConta()//remove conta introduzida pelo user se saldo for 0
        {
            mostrarContas();
            int index = lerConta(false);
            if (index == -1)
                return; // user cancelou
            if (Contas[index].saldo != 0) //saldo diferente de 0, cancela operação
            {
                printBox("Conta ainda tem saldo!");
                aguardarInput();
                return;
            }
            string s;
            do
            {
                printBox($"Tem a certeza que pertende eliminar esta conta? (sim/não)");
                Console.Write(" + ");
                s = Console.ReadLine();
            } while (s != "sim" && s != "não"); //confirmação
            if (s == "não")
                return;
            Contas.RemoveAt(index); //remove de Contas
        }
        static void mostrarContasDet()//Resumo detalhado das contas ativas
        {
            printBox("Contas Ativas");
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
        static void Main(string[] args)
        {
            //BANCO
            addAccounts(10);
            do
            {
                desenharMenu();
                int opcao = lerOpcaoMenu();
                Console.Clear();
                if (!verifTemContas(opcao))//verifica se existem contas para as opções pertinentes
                {
                    Console.WriteLine("");
                    Console.WriteLine(" Crie contas primeiro!");
                    aguardarInput();
                }
                else
                {
                    switch (opcao)
                    {
                        case 0:
                            return;
                        case 1:
                            levantar();
                            break;
                        case 2:
                            depositar();
                            break;
                        case 3:
                            transferir();
                            break;
                        case 4:
                            emitirSaldo();
                            break;
                        case 5:
                            emitirExtrato();
                            break;
                        case 6:
                            criarConta();
                            break;
                        case 7:
                            alterarConta();
                            break;
                        case 8:
                            removerConta();
                            break;
                        case 9:
                            mostrarContasDet();
                            aguardarInput();
                            break;
                        default:
                            break;
                    }
                }

            } while (true);
        }
    }
}
