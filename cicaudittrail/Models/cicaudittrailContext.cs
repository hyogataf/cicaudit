using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;

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
            Database.SetInitializer<cicaudittrailContext>(null);
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var schemaName = ConfigurationManager.AppSettings["schemaName"];

            modelBuilder.Entity<Role>().ToTable("ROLE", schemaName);
            modelBuilder.Entity<User>().ToTable("USER", schemaName);
            modelBuilder.Entity<CicRequest>().ToTable("CICREQUEST", schemaName);
            modelBuilder.Entity<CicRequestExecution>().ToTable("CICREQUESTEXECUTION", schemaName);
            modelBuilder.Entity<CicRequestResults>().ToTable("CICREQUESTRESULTS", schemaName);
            modelBuilder.Entity<CicRequestResultsFollowed>().ToTable("CICREQUESTRESULTSFOLLOWED", schemaName);
            modelBuilder.Entity<CicMessageTemplate>().ToTable("CICMESSAGETEMPLATE", schemaName);
            modelBuilder.Entity<CicMessageMail>().ToTable("CICMESSAGEMAIL", schemaName);
            modelBuilder.Entity<CicMessageMailDocuments>().ToTable("CICMESSAGEMAILDOCUMENTS", schemaName);
            modelBuilder.Entity<CicFollowedPropertiesValues>().ToTable("CICFOLLOWEDPROPERTIESVALUES", schemaName);
            modelBuilder.Entity<CicDiversRequestResults>().ToTable("CICDIVERSREQUESTRESULTS", schemaName);
            modelBuilder.Entity<CicRole>().ToTable("CICROLE", schemaName);
            modelBuilder.Entity<CicUserRole>().ToTable("CICUSERROLE", schemaName);

            //HasMany: CicRequestResultsFollowed hasMany CicMessageMails
            modelBuilder.Entity<CicRequestResultsFollowed>().HasMany(e => e.CicMessageMails).WithOptional(a => a.CicRequestResultsFollowed).HasForeignKey(e => e.CicRequestResultsFollowedId);

            //HasMany: CicMessageMails hasMany CicMessageMailDocuments
            modelBuilder.Entity<CicMessageMail>().HasMany(e => e.CicMessageMailDocuments).WithOptional(a => a.CicMessageMail).HasForeignKey(e => e.CicMessageMailId);

            //CicRequestExecution: CentifFile nullable
            modelBuilder.Entity<CicRequestExecution>().Property(m => m.CentifFile).IsOptional();
            base.OnModelCreating(modelBuilder);
            // modelBuilder.Conventions.Remove<ColumnTypeCasingConvention>();

        }

        public DbSet<cicaudittrail.Models.Role> Role { get; set; }

        public DbSet<cicaudittrail.Models.User> User { get; set; }

        public DbSet<cicaudittrail.Models.CicRequest> CicRequest { get; set; }

        public DbSet<cicaudittrail.Models.CicRequestExecution> CicRequestExecution { get; set; }

        public DbSet<cicaudittrail.Models.CicRequestResults> CicRequestResults { get; set; }

        public DbSet<cicaudittrail.Models.CicRequestResultsFollowed> CicRequestResultsFollowed { get; set; }

        public DbSet<cicaudittrail.Models.CicMessageTemplate> CicMessageTemplate { get; set; }

        public DbSet<cicaudittrail.Models.CicMessageMail> CicMessageMail { get; set; }

        public DbSet<cicaudittrail.Models.CicMessageMailDocuments> CicMessageMailDocuments { get; set; }

        public DbSet<cicaudittrail.Models.CicFollowedPropertiesValues> CicFollowedPropertiesValues { get; set; }

        public DbSet<cicaudittrail.Models.CicDiversRequestResults> CicDiversRequestResults { get; set; }

        public DbSet<cicaudittrail.Models.CicRole> CicRole { get; set; }

        public DbSet<cicaudittrail.Models.CicUserRole> CicUserRole { get; set; }
    }
}