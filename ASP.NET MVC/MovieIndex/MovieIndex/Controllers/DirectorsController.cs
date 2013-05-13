using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

using MovieIndex.Models;

namespace MovieIndex.Controllers
{
    public class DirectorsController : ApiController
    {
        private IDirectorRepository _repository;

        public DirectorsController(  )
        {
            _repository = new DirectorRepository( );
        }

        public DirectorsController( IDirectorRepository directorRepository )
        {
            _repository = directorRepository;
        }

        public async Task<IEnumerable<string>> GetAllDirectors( )
        {
            var result = await _repository.GetAllAsync( ); 
            
            return ( from d in result
                     select d.FullName );
        }

        public async Task<IEnumerable<string>> GetDirectorsByName( string name )
        {
            var result = await _repository.GetByNameAsync( name );

            return ( from d in result
                     select d.FullName );
        }
    }
}