using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrasesDeTodos.Models
{
    public class Autor
    {
        public int AutorId { get; set; }

        public string Nombre { get; set; }

        public List<Frase> Frases {get; set;}

        public Autor()
        {
            Frases = new List<Frase>();
        }
    }
}