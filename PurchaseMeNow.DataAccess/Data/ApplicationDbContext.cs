using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PurchaseMeNow.Models;

namespace PurchaseMeNow.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<ApprovalDecision> ApprovalDecisions { get; set; }

        public DbSet<PurchaseDecision> PurchaseDecisions { get; set; }

        public DbSet<DeliveryDetail> DeliveryDetails { get; set; }
    }
}
