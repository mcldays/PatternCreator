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




    }
}