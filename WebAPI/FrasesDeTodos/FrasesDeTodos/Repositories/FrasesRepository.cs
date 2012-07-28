using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FrasesDeTodos.Models;

namespace FrasesDeTodos.Repositories
{
	public static class FrasesRepository
	{
		private static List<Frase> _frases;
		private static Dictionary<string, List<Frase>> _frasesPorPersona;

		static FrasesRepository()
		{
			_frases = new List<Frase>();
			_frasesPorPersona = new Dictionary<string, List<Frase>>();
		}

		public static Frase NuevaFrase(string frase, string autor)
		{
			Frase f = new Frase();
			f.Texto = frase;
			f.Id = _frases.Count + 1;

			if (!_frasesPorPersona.ContainsKey(autor))
			{
				_frasesPorPersona.Add(autor, new List<Frase>());
			}

			_frasesPorPersona[autor].Add(f);
			_frases.Add(f);

			return f;
		}

		public static List<Frase> ObtenerFrases(string autor)
		{
			if (_frasesPorPersona.ContainsKey(autor))
			{
				return new List<Frase>(_frasesPorPersona[autor].OrderByDescending(f => f.Id));
			}
			else
			{
				return new List<Frase>();
			}
		}

		public static void Votar(int idFrase)
		{
			var frase = _frases.FirstOrDefault(f => f.Id == idFrase);
			if (frase != null)
			{
				frase.Votos++;
			}
		}

		public static void Comentar(int idFrase, string comentario)
		{
			var frase = _frases.FirstOrDefault(f => f.Id == idFrase);
			if (frase != null)
			{
				frase.Comentarios.Add(comentario);
			}
		}
	}
}