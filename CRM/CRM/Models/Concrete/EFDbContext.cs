using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CRM.Models.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext() : base("name=MiniCRMEntities") { }
        public DbSet<Users> Users { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Managers> Managers { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Images> Images { get; set; }
    }
}