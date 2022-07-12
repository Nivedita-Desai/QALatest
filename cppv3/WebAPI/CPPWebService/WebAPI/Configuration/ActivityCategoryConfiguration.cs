using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.Configuration
{
    public class ActivityCategoryConfiguration : EntityTypeConfiguration<ActivityCategory>
    {
        public ActivityCategoryConfiguration():base(){
            ToTable("activity_category");
        }
    }
}