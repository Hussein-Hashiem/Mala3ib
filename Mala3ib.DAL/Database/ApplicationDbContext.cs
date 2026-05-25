
namespace Mala3ib.DAL.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<FieldOwner> FieldOwners { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldImage> FieldImages { get; set; }
        public DbSet<FieldSlot> FieldSlots { get; set; }
        public DbSet<FieldSlotPlayer> FieldSlotPlayers { get; set; }
        public DbSet<FieldReview> FieldReviews { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<EmailVerficationOtp> EmailVerficationOtps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            // Application User with Admin & FieldOwner & Player
            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<Player>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FieldOwner>()
                .HasOne(f => f.User)
                .WithOne()
                .HasForeignKey<FieldOwner>(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Follow
            modelBuilder.Entity<Follow>()
            .HasOne(x => x.Follower)
            .WithMany(x => x.Following)
            .HasForeignKey(x => x.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(x => x.Following)
                .WithMany(x => x.Followers)
                .HasForeignKey(x => x.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Fields & FieldOwner
            modelBuilder.Entity<Field>()
                .HasOne(f => f.FieldOwner)
                .WithMany(fo => fo.Fields)
                .HasForeignKey(f => f.FieldOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // FieldImages
            modelBuilder.Entity<FieldImage>()
                .HasOne(fi => fi.Field)
                .WithMany(f => f.Images)
                .HasForeignKey(fi => fi.FieldId)
                .OnDelete(DeleteBehavior.Cascade);

            // FieldSlots
            modelBuilder.Entity<FieldSlot>()
                .HasOne(fs => fs.Field)
                .WithMany(f => f.Slots)
                .HasForeignKey(fs => fs.FieldId)
                .OnDelete(DeleteBehavior.Restrict);


            // FieldReviews
            modelBuilder.Entity<FieldReview>()
                .HasOne(fr => fr.Field)
                .WithMany(f => f.Reviews)
                .HasForeignKey(fr => fr.FieldId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FieldReview>()
                .HasOne(fr => fr.Player)
                .WithMany(p => p.FieldReviews)
                .HasForeignKey(fr => fr.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invitaions
            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Sender)
                .WithMany()
                .HasForeignKey(i => i.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Reciever)
                .WithMany()
                .HasForeignKey(i => i.RecieverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Player and fieldSlot
            modelBuilder.Entity<FieldSlotPlayer>()
                .HasOne(x => x.FieldSlot)
                .WithMany(x => x.Players)
                .HasForeignKey(x => x.FieldSlotId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FieldSlotPlayer>()
                .HasOne(x => x.Player)
                .WithMany()
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}