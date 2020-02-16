using System;
using System.Collections.Generic;
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

        public DbSet<AutModel> AutModels { get; set; }

        public DbSet<CompanyModel> CompanyModels { get; set; }

        public  DbSet<UserModel> UserModels { get; set;}

        public DbSet<PictureModel> PicturesModels { get; set; }

        public DbSet<PositionModel> PositionModels { get; set; }


    }
}