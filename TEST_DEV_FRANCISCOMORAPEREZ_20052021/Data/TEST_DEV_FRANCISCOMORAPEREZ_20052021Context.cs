using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TEST_DEV_FRANCISCOMORAPEREZ_20052021.Models;

namespace TEST_DEV_FRANCISCOMORAPEREZ_20052021.Data
{
    public class TEST_DEV_FRANCISCOMORAPEREZ_20052021Context : DbContext
    {
        public TEST_DEV_FRANCISCOMORAPEREZ_20052021Context (DbContextOptions<TEST_DEV_FRANCISCOMORAPEREZ_20052021Context> options)
            : base(options)
        {
        }

        public DbSet<TEST_DEV_FRANCISCOMORAPEREZ_20052021.Models.PersonasFisicasModel> PersonasFisicasModel { get; set; }
    }
}
