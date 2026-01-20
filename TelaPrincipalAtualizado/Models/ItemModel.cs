using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelaPrincipalAtualizado.Models
{
    public class ItemModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Nome { get; set; } = string.Empty;
        public string PrecoPorDia { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Localizacao { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public DateTime DataVisualizacao { get; set; } = DateTime.Now;
        public string Tipo { get; set; } = string.Empty;
    }
}
