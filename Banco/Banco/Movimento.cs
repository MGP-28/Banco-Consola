using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ficha_2
{
    internal class Movimento
    {
        public double valor { get; set; }
        public bool debito { get; set; }
        public string descricao { get; set; }
        //Construtores
        public Movimento()
        {
            this.valor = 0;
            this.debito = false;
            this.descricao = "Movimento em branco";
        }
        public Movimento(double valor, bool debito)
        {
            this.valor = valor;
            this.debito = debito;
            this.descricao = "S/ descrição";
        }
        public Movimento(double valor, bool debito, string descricao)
        {
            this.valor = valor;
            this.debito = debito;
            this.descricao = descricao;
        }
        public Movimento(string descricao)
        {
            this.valor = 0;
            this.debito = false;
            this.descricao = descricao;
        }

        //overrides
        public override string ToString()
        {
            string text = $"  {descricao}\n  Tipo: ";
            if (debito)
                text = text + "Débito\n  Valor: -";
            else
                text = text + "Crédito\n  Valor: +";
            text = text + valor.ToString("0.00") + " €";
            return text;
        }
        //movimento igual ignora descricao
        public override bool Equals(object obj)
        {
            Movimento mov = obj as Movimento;
            if (mov.valor == valor && mov.debito == debito)
                return true;
            else
                return false;
        }
    }
}
