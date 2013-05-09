using System.Collections.Generic;

namespace MovieIndex.Models
{
    public interface IGenreRepository
    {
        List<Genre> GetAll( );
    }
}