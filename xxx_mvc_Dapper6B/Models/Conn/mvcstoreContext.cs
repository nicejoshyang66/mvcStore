

using Microsoft.EntityFrameworkCore;

namespace mvc_Dapper.Models.Conn
{
    public partial class mvcstoreContext:DbContext
    {
        public mvcstoreContext()
        {
            
        }

        public mvcstoreContext(DbContextOptions<mvcstoreContext>options):base(options)
        {
            
        }
        
    }
}