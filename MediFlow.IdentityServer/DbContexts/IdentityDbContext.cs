using Marvin.IDP.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marvin.IDP.DbContexts
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserClaim> UserClaims { get; set; }         

        public IdentityDbContext(
          DbContextOptions<IdentityDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Subject)
            .IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                    Password = "password",
                    Subject = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    UserName = "David",
                    Active = true,
                    ConcurrencyStamp = new Guid("b4c9b3f2-3f93-4ad4-b296-9d247f09e589").ToString(),
                    Email = "David@gmail.com"
				},
                new User()
                {
                    Id = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                    Password = "password",
                    Subject = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    UserName = "Emma",
                    Active = true,
					ConcurrencyStamp = new Guid("1d183f91-2c2a-4b49-9f2d-37a1a3f4fc7b").ToString(),
					Email = "Emma@gmail.com"
				});

            modelBuilder.Entity<UserClaim>().HasData(
             new UserClaim()
             {
                 Id = new Guid("e3d7a444-84c0-4ff6-956e-6a8c9bce0a23"),
                 UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                 Type = "given_name",
                 Value = "David",
				 ConcurrencyStamp = new Guid("a7b4a1cb-65fd-4a39-b6c1-3c2f8374c48d").ToString()
			 },
             new UserClaim()
             {
                 Id = new Guid("a6f8b9c1-1c8d-4b2e-925e-5f3d1328f23f"),
                 UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                 Type = "family_name",
                 Value = "Flagg",
				 ConcurrencyStamp = new Guid("e582a3b4-f7b0-4b44-934a-5f3c8f8f13b1").ToString()
			 }, 
             new UserClaim()
             {
                 Id = new Guid("7b219d12-9f75-466e-91ff-27b308ab6bc5"),
                 UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                 Type = "country",
                 Value = "nl",
				 ConcurrencyStamp = new Guid("cb01b61c-7e12-4cd1-9a55-8f2ce6a47e29").ToString()
			 },
             new UserClaim()
             {
                 Id = new Guid("c34e2d14-7d12-4ecf-b81c-c5b4f2dd3ba8"),
                 UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                 Type = "role",
                 Value = "FreeUser",
				 ConcurrencyStamp = new Guid("9c8e2dd4-6b22-46ea-b6a1-91bcb7dc5d73").ToString()
			 },
             new UserClaim()
             {
                 Id = new Guid("5d2cfa90-3c4e-4c41-93e1-7e2f96b8129e"),
                 UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                 Type = "given_name",
                 Value = "Emma",
				 ConcurrencyStamp = new Guid("dc18bce9-78c4-4c4e-81c1-6e4e92257977").ToString()
			 },
             new UserClaim()
             {
                 Id = new Guid("1a9f33e2-79ef-4f6f-b64a-44fbb4d8a215"),
                 UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                 Type = "family_name",
                 Value = "Flagg",
				 ConcurrencyStamp = new Guid("a12fb40a-b2dc-42a5-93f0-963986129d48").ToString()
			 }, 
             new UserClaim()
             {
                 Id = new Guid("b8e5a570-114b-4a56-8b7e-3b7a86f0b9c6"),
                 UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                 Type = "country",
                 Value = "be",
				 ConcurrencyStamp = new Guid("7f405a35-e654-4c5a-a6b5-110e87e9a9ff").ToString()
			 }, 
             new UserClaim()
             {
                 Id = new Guid("f32751a9-c9da-40b3-8434-2bfa163acc79"),
                 UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                 Type = "role",
                 Value = "PayingUser",
				 ConcurrencyStamp = new Guid("4067e2f4-b109-4a9f-8bd2-3bce0a1f2d5d").ToString()
			 });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // get updated entries
            var updatedConcurrencyAwareEntries = ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified)
                    .OfType<IConcurrencyAware>();

            foreach (var entry in updatedConcurrencyAwareEntries)
            {
                entry.ConcurrencyStamp = Guid.NewGuid().ToString();
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
