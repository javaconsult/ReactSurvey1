using ClassLibrary1.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary1.Repository
{
    public class SurveyDbContext: IdentityDbContext<AdminUser>
    {
        public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options)
        {
        }


        public DbSet<Question>? Questions { get; set; }
        public DbSet<Option>? Options { get; set; }
        public DbSet<Survey>? Surveys { get; set; }
        public DbSet<Participant>? Participants { get; set; }
        public DbSet<Selection>? Selections { get; set; }
        public DbSet<AdminUser>? AdminUsers {  get; set; }

    }
}
