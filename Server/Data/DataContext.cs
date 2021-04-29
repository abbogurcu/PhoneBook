using Microsoft.EntityFrameworkCore;
using Rehberv2.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rehberv2.Server.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Rehber> Rehber { get; set; }
    }
}
