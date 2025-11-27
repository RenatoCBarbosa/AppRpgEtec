using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.Models
{
    public class Arma
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Dano { get; set; }
        public int PersonagemId { get; set; }
    }
}
