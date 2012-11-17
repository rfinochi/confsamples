using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrasesDeTodos.Models
{
    public class Voto
    {
        public int VotoId { get; set; }

        public int FraseId { get; set; }

        public Frase Frase { get; set; }

        public bool MeGusta { get; set; }

        public DateTime Fecha { get; set; }
    }
}