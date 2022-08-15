using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgDouble.Models
{
    public class OrgDoubleContex: DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public OrgDoubleContex(DbContextOptions<OrgDoubleContex> options)
           : base(options)
        {
      /* Database.EnsureDeleted();*/
            Database.EnsureCreated();   
        }
    }
}
