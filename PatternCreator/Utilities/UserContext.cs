using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

using Microsoft.EntityFrameworkCore;
using PatternCreator.Models;

namespace PatternCreator.Utilities
{
    public class UserContext : DbContext
    {
        public UserContext() : base("DefaultConnection")
        {

        }
        public DbSet<AutoTextModel> AutoTexts { get; set; }

        public DbSet<AutModel> AutModels { get; set; }

        public DbSet<CompanyModel> CompanyModels { get; set; }

        public DbSet<UserModel> UserModels { get; set;}
        public DbSet<SpecialtyModel> Specialties { get; set; }
        public DbSet<ProtocolModel> Protocols { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<DocumentModel> DocumentModels { get; set; }
        public DbSet<PictureModel> PicturesModels { get; set; }
        public DbSet<StampModel> StampModels { get; set; }
        public DbSet<StampPositions> StampPositions { get; set; }
        public DbSet<PositionModel> PositionModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ProtocolModel>().HasIndex(p => new
            //    {p.Date, p.DocumentModels}).HasName("IndexOfDay").IsUnique(true);
        }
    }
}