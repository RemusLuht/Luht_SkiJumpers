using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Luht_SkiJumpers.Models;

namespace Luht_SkiJumpers.Data
{
    public class Luht_SkiJumpersContext : DbContext
    {
        public Luht_SkiJumpersContext (DbContextOptions<Luht_SkiJumpersContext> options)
            : base(options)
        {
        }

        public DbSet<Luht_SkiJumpers.Models.Jumpers> Jumpers { get; set; } = default!;
    }
}
