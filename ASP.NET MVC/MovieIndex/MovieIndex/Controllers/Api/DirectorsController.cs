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

        /// <summary>
        /// Get all movie directors names
        /// </summary>
        /// <returns>String collection of movie directors names</returns>
        public async Task<IEnumerable<string>> GetAllDirectors( )
        {
            var result = await _repository.GetAllAsync( ); 
            
            return ( from d in result
                     select d.FullName );
        }

        /// <summary>
        /// Get director that match with the parameter
        /// </summary>
        /// <param name="name">Director name to search</param>
        /// <returns>String collection of movie directors names</returns>
        public async Task<IEnumerable<string>> GetDirectorsByName( string name )
        {
            var result = await _repository.GetByNameAsync( name );

            return ( from d in result
                     select d.FullName );
        }
    }
}