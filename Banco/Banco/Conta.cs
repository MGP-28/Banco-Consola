using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ficha_2
{
    internal class Conta
    {
        public int id { get; set; }
        public double saldo { get; set; }
        public bool especial { get; set; }
        public double limiar { get; set; }
        public List<Movimento> Movimentacao { get; set; }
        //Construtores
        public Conta()
        {
            this.id = id;
            this.saldo = 0;
            this.especial = especial;
            this.limiar = 0;
            this.Movimentacao = new List<Movimento>();
        }
        public Conta(int id)
        {
            this.id = id;
            this.saldo = 0;
            this.especial = especial;
            this.limiar = 0;
            this.Movimentacao = new List<Movimento>();
        }
        public Conta(int id, double saldo, bool especial, double limite) : this(id)
        {
            this.id = id;
            this.saldo = 0;
            this.especial = especial;
            this.limiar = limite;
            this.Movimentacao = new List<Movimento>();
        }
        public Conta(int id, double saldo, bool especial, double limite, List<Movimento> movimentacao) : this(id, saldo, especial, limite)
        {
            this.id = id;
            this.saldo = 0;
            this.especial = especial;
            this.limiar = limite;
            this.Movimentacao = new List<Movimento>();
        }
        //overrides
    }
}
