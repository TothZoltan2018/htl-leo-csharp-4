using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuckNorrisExcercise.EFDataContext
{
    public class ChuckNorrisContext : DbContext
    {
        public ChuckNorrisContext(DbContextOptions<ChuckNorrisContext> options)
            : base(options) { }

        public DbSet<JokeData> JokeDatas { get; set; }        
    }
} 
