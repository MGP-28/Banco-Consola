using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ficha_2
{
    internal class ID
    {
        private static Random rndGlobal = new Random();
        private static int idGlobal = 1000;
        public static int newID()
        {
            idGlobal += rndGlobal.Next(1, 30);
            return idGlobal;
        }
    }
    internal class Program
    {
        static List<Conta> Contas = new List<Conta>();
        static void addAccounts(int n)
        {
            Random rndT = new Random();
            for (int i = 0; i < n - 2; i++)
            {
                Contas.Add(new Conta(ID.newID(), 0, false, rndT.Next(0, 1000)));
                double saldo = rndT.Next(0, 1500);
                Contas[i].saldo = saldo;
                Contas[i].Movimentacao.Add(new Movimento(saldo, false, "Saldo inicial"));
            }
            //especial
            Contas.Add(new Conta(ID.newID(), 0, true, rndT.Next(0, 1000)));
            Contas[8].saldo = rndT.Next(0, 1500);
            //negativa
            Contas.Add(new Conta(ID.newID(), 0, false, rndT.Next(0, 1000)));
            Contas[9].saldo = rndT.Next(-250, -50);
            hasAccount = true;
        }
        static bool mainLoop = true;
        static bool hasAccount = false;
        static void aguardarInput()
        {
            Console.WriteLine("\n ENTER para Continuar");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
        static void desenharMenu()
        {
            Console.Clear();
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
        static int lerOpcaoMenu()
        {
            string key = "";
            do
            {
                key = key + Console.ReadKey(true).KeyChar;
            } while (!int.TryParse(key, out _));
            int result = int.Parse(key);
            return result;
        }
        static bool verifTemContas(int opcao)
        {
            if (new[] { 1, 2, 3, 4, 5, 7, 8 }.Contains(opcao) && !hasAccount)
                return false;
            else
                return true;
        }
        static int readInt()
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
        static int intIntroduzir(string title)
        {
            Console.WriteLine(" +---------------------------------------------------+");
            Console.WriteLine(" | {0}|", title.PadRight(50));
            Console.WriteLine(" +---------------------------------------------------+");
            return readInt();
        }
        static double doubleIntroduzir(string title)
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
        static int verifConta(int id)
        {
            for (int i = 0; i < Contas.Count(); i++)
            {
                if (Contas[i].id == id)
                    return i;
            }
            return -1;
        }
        static int lerConta(bool credito)
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
                    return -1;
                index = verifConta(id);
                if (index < 0)
                    Console.WriteLine("\n Conta inválida");
                else if (index == 0)
                    return 0;
            } while (index < 0);
            return index;
        }
        static int verifSaldo(int index, double value)
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
        static void mostrarContas()
        {
            printBox("Contas Ativas");
            int cnt = 0;
            foreach (Conta c in Contas)
            {
                if (cnt == 0)
                    Console.Write(" | ");
                Console.Write("{0}", c.id.ToString().PadRight(12));
                if(cnt == 3)
                {
                    Console.Write("  |\n");
                    cnt = -1;
                }
                cnt++;
            }
            while (cnt < 4)
            {
                Console.Write("{0}", "".PadRight(12));
                cnt++;
            }
            Console.Write("  |\n");
            Console.WriteLine(" +---------------------------------------------------+");
        }
        static bool debitLogic(ref int index, ref double value)
        {
            index = lerConta(false);
            if (index == -1)
                return false;
            value = doubleIntroduzir(" Valor (0 para cancelar)");
            if (value == 0)
                return false;
            else
            {
                //-1 sem saldo, 1 saldo suficiente, 2 suficiente mas entra no limiar
                int status = verifSaldo(index, value);
                switch (status)
                {
                    case -1:
                        Console.WriteLine("\n !!! Saldo insuficiente !!!");
                        aguardarInput();
                        return false;
                    case 2:
                        Console.WriteLine("\n !!!!! ATENÇÃO !!!!!\n\n Prosseguir com a operação ativa o limiar da conta!\n\n Prima '7' para prosseguir ou '0' para cancelar.");
                        int n;
                        do
                        {
                            n = readInt();
                        } while (n != 7 && n != 0);
                        if (n == 0)
                            return false;
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
        static void levantar()
        {
            mostrarContas();
            int index = -1; double value = -1;
            if (!debitLogic(ref index, ref value))
                return;
            Contas[index].Movimentacao.Add(new Movimento(value, true, "Levantamento " + DateTime.Now.ToString()));
            Contas[index].saldo -= value;
            printBox($"Levantou {value.ToString("0.00")} € da conta {Contas[index].id}!");
            aguardarInput();
        }
        static void depositar()
        {
            mostrarContas();
            int index = lerConta(true);
            if (index == -1)
                return;
            double value = doubleIntroduzir(" Valor (0 para cancelar)");
            if (value == 0)
                return;
            Contas[index].Movimentacao.Add(new Movimento(value, false, "Deposito " + DateTime.Now.ToString()));
            Contas[index].saldo += value;
            printBox($"Depositou {value.ToString("0.00")} € da conta {Contas[index].id}!");
            aguardarInput();
        }
        static void transferir()
        {
            mostrarContas();
            int indexSaida = -1; double value = -1;
            if (!debitLogic(ref indexSaida, ref value))
                return;
            //ler conta a creditar
            int indexEntrada = lerConta(true);
            if (indexEntrada == -1)
                return;
            //debitar
            Contas[indexSaida].Movimentacao.Add(new Movimento(value, true, $"Transferência PARA {Contas[indexSaida].id} " + DateTime.Now.ToString()));
            Contas[indexSaida].saldo -= value;
            //creditar
            Contas[indexEntrada].Movimentacao.Add(new Movimento(value, false, $"Transferência DE {Contas[indexEntrada].id} " + DateTime.Now.ToString()));
            Contas[indexEntrada].saldo += value;

            printBox($"Transferiu {value.ToString("0.00")} € de {Contas[indexSaida].id} para {Contas[indexEntrada].id}!");
            aguardarInput();
        }
        static void printBox(string title)
        {
            Console.WriteLine(" +---------------------------------------------------+");
            Console.WriteLine(" | {0}|", title.PadRight(50));
            Console.WriteLine(" +---------------------------------------------------+");
        }
        static void emitirSaldo()
        {
            mostrarContas();
            int index = lerConta(false);
            printBox($"Saldo: {Contas[index].saldo} €");
            aguardarInput();
        }
        static void emitirExtrato()
        {
            //Não imprime conteúdo!!!
            mostrarContas();
            int index = lerConta(false);
            if (index == -1)
                return;
            printBox("Extrato: ");
            foreach (Movimento mov in Contas[index].Movimentacao)
            {
                Console.WriteLine(mov.ToString());
                Console.WriteLine(" +---------------------------------------------------+");
            }
            aguardarInput();
        }
        static void criarConta()
        {
            string s;
            do
            {
                printBox("Conta especial? (sim / não)");
                Console.Write(" + ");
                s = Console.ReadLine();
            } while (s != "sim" && s != "não");
            bool specialAcc = false;
            if (s == "sim")
                specialAcc = true;
            double limiar = doubleIntroduzir("Qual o limiar autorizado?");
            int nConta;
            do
            {
                nConta = ID.newID();
            } while (verifConta(nConta) != -1);
            Conta conta = new Conta(nConta, 0, specialAcc, limiar);
            Contas.Add(conta);
            printBox($"Conta nº {nConta} criada!");
            aguardarInput();
        }
        static void contaEspecialTroca(int index)
        {
            string opt = "não ";
            if (Contas[index].especial)
                opt = "";
            string s;
            do
            {
                printBox($"A conta atual {opt}é especial. Trocar? (sim/não)");
                Console.Write(" + ");
                s = Console.ReadLine();
            } while (s != "sim" && s != "não");
            Contas[index].especial = (Contas[index].especial) ? false : true;
        }
        static void alterarConta()
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
            string key = "";
            do
            {
                key = key + Console.ReadKey(true).KeyChar;
            } while (key != "1" && key != "2");
            int opcao = int.Parse(key);
            if (opcao == 1)
            {
                contaEspecialTroca(index);
            }
            else
            {
                printBox($"Limiar Atual: {Contas[index].limiar}€");
                Contas[index].limiar = doubleIntroduzir($"Introduza o novo limiar");
            }
        }
        static void removerConta()
        {
            mostrarContas();
            int index = lerConta(false);
            if (index == -1)
                return;
            if (Contas[index].saldo != 0)
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
            } while (s != "sim" && s != "não");
            if (s == "não")
                return;
            Contas.RemoveAt(index);
        }
        static void mostrarContasDet()
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
                if (!verifTemContas(opcao))
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
                            mainLoop = false;
                            break;
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

            } while (mainLoop);
        }
    }
}
