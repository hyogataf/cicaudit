using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace cicaudittrail.Models
{
    public class cicaudittrailContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<cicaudittrail.Models.cicaudittrailContext>());

        public cicaudittrailContext()
            : base(new OracleConnection(ConfigurationManager.ConnectionStrings["cicaudittrailContext"].ConnectionString), true) 
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable("ROLE", "BANK");
            modelBuilder.Entity<User>().ToTable("USER", "BANK");
            modelBuilder.Entity<CicRequest>().ToTable("CICREQUEST", "BANK");
            modelBuilder.Entity<CicRequestExecution>().ToTable("CICREQUESTEXECUTION", "BANK");
            modelBuilder.Entity<CicRequestResults>().ToTable("CICREQUESTRESULTS", "BANK");
            modelBuilder.Entity<CicRequestResultsFollowed>().ToTable("CICREQUESTRESULTSFOLLOWED", "BANK");
           // modelBuilder.Conventions.Remove<ColumnTypeCasingConvention>();

        }

        public DbSet<cicaudittrail.Models.Role> Role { get; set; }

        public DbSet<cicaudittrail.Models.User> User { get; set; }

        public DbSet<cicaudittrail.Models.CicRequest> CicRequest { get; set; }

        public DbSet<cicaudittrail.Models.CicRequestExecution> CicRequestExecution { get; set; }

        public DbSet<cicaudittrail.Models.CicRequestResults> CicRequestResults { get; set; }

        public DbSet<cicaudittrail.Models.CicRequestResultsFollowed> CicRequestResultsFollowed { get; set; }
    }
}