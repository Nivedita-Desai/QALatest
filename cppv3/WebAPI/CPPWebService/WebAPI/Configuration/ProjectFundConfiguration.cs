using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.Configuration
{
    public class ProjectFundConfiguration: EntityTypeConfiguration<ProjectFund>
    {
        public ProjectFundConfiguration() : base()
        {
            HasKey(p => p.ProjectID);
            ToTable("project_fund");
            Property(p => p.FundName);
            Property(p => p.FundAmount);
            Property(p => p.AppliedDate);
            Property(p => p.ProjectID);
        }

    }
}