using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Infrastructure.Presenters.DbContext
{
    public class SeedingData
    {
        private readonly ModelBuilder modelBuilder;

        public SeedingData(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            throw new NotImplementedException();
        }
    }
}
