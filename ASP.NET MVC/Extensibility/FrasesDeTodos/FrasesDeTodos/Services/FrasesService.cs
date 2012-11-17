using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FrasesDeTodos.Models;

namespace FrasesDeTodos.Services
{
	public static class FrasesService
	{
		private static List<Frase> _frases;
		private static List<Autor> _autores;

		static FrasesService()
		{
			var krako = new Autor() { Nombre = "Krako", AutorId = 1 };
            var mariano = new Autor() { Nombre = "Mariano", AutorId = 2 };

			_autores = new List<Autor>();

            _autores.Add(krako);
            _autores.Add(mariano);

			_frases = new List<Frase>();

			_frases.Add(new Frase() { Texto = "Sharepoint es como una catapulta de conejos, con la que cada tanto le pegas a tu solución", FraseId = 1, Autor = krako });
			_frases.Add(new Frase() { Texto = "Basta de peliculas de HP, el pibe ya no es un pibe! Como se va a llamar la prox?, Harry Potter y el examen de Prostata???", FraseId = 2, Autor = krako });
			_frases.Add(new Frase() { Texto = "Los hombres somos COM y las mujeres CORBA lo único que podemos intercambiar es un poco de texto plano", FraseId = 3, Autor = krako });
		}

        public static Autor ObtenerAutor(string nombreAutor)
        {
            var autor = _autores.Where(x => x.Nombre == nombreAutor).FirstOrDefault();

            if (autor == null)
            {
                autor = new Autor() { Nombre = nombreAutor, AutorId = _autores.Count() + 1 };
                _autores.Add(autor);

            }

            return autor;
        }

		public static Frase NuevaFrase(string texto, string nombreAutor)
		{
            var autor = ObtenerAutor(nombreAutor);

			var frase = new Frase();
			frase.Texto = texto;
			frase.FraseId = _frases.Count + 1;
			frase.Autor = autor;
            frase.AutorId = autor.AutorId;

            NuevaFrase(frase);

            autor.Frases.Add(frase);

			return frase;
		}

        public static Frase NuevaFrase(Frase frase)
        {
            _frases.Add(frase);

            return frase;
        }

		public static List<Frase> ObtenerFrases(string nombreAutor)
		{
			return _frases.Where(x => x.Autor.Nombre.ToLower() == nombreAutor.ToLower()).OrderBy(f => f.FraseId).ToList();
		}

        public static List<Frase> ObtenerFrases()
        {
            return _frases.ToList();
        }

		public static int Votar(int fraseId, bool meGusta)
		{
			var frase = _frases.FirstOrDefault(f => f.FraseId == fraseId);
			if (frase != null)
			{
				var voto = new Voto() { MeGusta = meGusta, Fecha = DateTime.Now };

				frase.Votos.Add(voto);

				voto.VotoId = fraseId * 1000 + frase.Votos.Count();
			}

			return frase.CantidadVotos;
		}

		public static Comentario Comentar(int fraseId, string comentario)
		{
			var frase = _frases.FirstOrDefault(f => f.FraseId == fraseId);

			if (frase == null)
				return null;

			var comentarioFrase = new Comentario() { Texto = comentario, FechaCreacion = DateTime.Now };

			frase.Comentarios.Add(comentarioFrase);

			comentarioFrase.ComentarioId = fraseId * 1000 + frase.Comentarios.Count();

			return comentarioFrase;
		}

		public static void EliminarComentario(int fraseId, int comentarioId)
		{
			var frase = _frases.FirstOrDefault(f => f.FraseId == fraseId);
			if (frase != null)
			{
				frase.Comentarios.Remove(
					frase.Comentarios.Where(x => x.ComentarioId == comentarioId).FirstOrDefault());
			}
		}

		public static Comentario EditarComentario(int fraseId, int comentarioId, string comentario)
		{
			var frase = _frases.FirstOrDefault(f => f.FraseId == fraseId);
			if (frase == null)
				return null;

			var current = frase.Comentarios.Where(x => x.ComentarioId == comentarioId).FirstOrDefault();

			if (current != null)
				current.Texto = comentario;

			return current;
		}

	}
}