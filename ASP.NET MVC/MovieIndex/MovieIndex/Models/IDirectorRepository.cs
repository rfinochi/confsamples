using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieIndex.Models
{
    public interface IDirectorRepository
    {
        Task<List<Director>> GetAllAsync( );

        Task<List<Director>> GetByNameAsync( string name );
    }
}