using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class PdfDbContext : DbContext
    {
        public PdfDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Pdf> Pdf { get; set; }
    }
}
