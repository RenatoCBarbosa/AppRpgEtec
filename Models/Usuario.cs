using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PassowordString { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[]? Foto { get; set; }
        public double? Latitude { get; set; } //? - pode ser algo ou nada
        public double? Longitude { get; set; }
    }
}
