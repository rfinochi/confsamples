using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrasesDeTodos.Models
{
    public class Comentario
    {
        public int ComentarioId { get; set; }

        public String Texto { get; set; }

        public int FraseId { get; set; }

        public Frase Frase { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}