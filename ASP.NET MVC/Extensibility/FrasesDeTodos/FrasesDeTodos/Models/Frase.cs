using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FrasesDeTodos.Models
{
    public class Frase
    {
        public Frase()
        {
            Comentarios = new List<Comentario>();
            Votos = new List<Voto>();
        }

        public int FraseId { get; set; }

        [Required(ErrorMessage="Si no vas a decir nada no molestes che!!!")]
        public string Texto { get; set; }
                
        public List<Voto> Votos { get; set; }

        public List<Comentario> Comentarios { get; set; }

        public int AutorId { get; set; }

        public Autor Autor { get; set; }

        public int CantidadVotos 
        {
            get { return CalcularVotos(); }
        }

        private int CalcularVotos()
        {
            var meGusta = Votos.Where(x => x.MeGusta).Count();

            var noMeGusta = Votos.Where(x => !x.MeGusta).Count();

            return meGusta - noMeGusta;
        }
    }
}