using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrasesDeTodos.Models
{
	public class Frase
	{
		public Frase()
		{
			Texto = String.Empty;
			Id = 0;
			Comentarios = new List<string>();
			Votos = 0;
		}

		public string Texto
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public int Votos
		{
			get;
			set;
		}

		public List<string> Comentarios
		{
			get;
			set;
		}
	}
}