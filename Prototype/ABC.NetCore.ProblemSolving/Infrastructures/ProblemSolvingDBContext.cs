using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ABC.NetCore.ProblemSolving.Models;

namespace ABC.NetCore.ProblemSolving.Infrastructures
{
    public class ProblemSolvingDBContext: DbContext
    {
        public ProblemSolvingDBContext(DbContextOptions<ProblemSolvingDBContext> options) : base(options)
        { }

        public DbSet<CustomerEntity> Customers { get; set; }

        public DbSet<ComplaintCodeEntity> ComplaintCodes { get; set; }

        public DbSet<DepartmentEntity> Departments { get; set; }

        public DbSet<PlantEntity> Plants { get; set; }

        public DbSet<ProblemSolvingTypeEntity> ProblemSolvingTypes { get; set; }
    }
}
