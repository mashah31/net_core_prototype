using Microsoft.EntityFrameworkCore;
using ABC.NetCore.ProblemSolving.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.NetCore.ProblemSolving.Infrastructures
{
    public class CentralDataDBContext: DbContext
    {
        public CentralDataDBContext(DbContextOptions<CentralDataDBContext> options) : base(options)
        { }
        
        public DbSet<SAPPartEntity> SAPParts { get; set; }

        public DbSet<SAPEmployeeEntity> SAPEmployees { get; set; }
    }
}
