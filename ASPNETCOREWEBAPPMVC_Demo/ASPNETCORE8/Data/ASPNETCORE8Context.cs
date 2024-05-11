using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace ASPNETCORE8.Data
{
    public class ASPNETCORE8Context : DbContext
    {
        public ASPNETCORE8Context (DbContextOptions<ASPNETCORE8Context> options)
            : base(options)
        {
        }

        public DbSet<MvcMovie.Models.Movie> Movie { get; set; } = default!;
    }
}
