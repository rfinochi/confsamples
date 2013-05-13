using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieIndex.Models
{
    public class DirectorRepository : IDirectorRepository
    {
        public async Task<List<Director>> GetAllAsync( )
        {
            return await Task.Run( ( ) =>
            {
                using ( MovieIndexContext context = new MovieIndexContext( ) )
                {
                    return ( from d in context.Directors
                             select d ).ToList( );
                }
            } ); 
        }

        public async Task<List<Director>> GetByNameAsync( string name )
        {
            return await Task.Run( ( ) =>
            {
                using ( MovieIndexContext context = new MovieIndexContext( ) )
                {
                    return ( from d in context.Directors
                             where d.FullName.ToLower( ).Contains( name.ToLower( ) )
                             select d ).ToList( );
                }
            } ); 
        }
    }
}