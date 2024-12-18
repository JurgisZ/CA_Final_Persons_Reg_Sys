using CA_Final_Persons_Reg_Sys.Model;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CA_Final_Persons_Reg_Sys.Data
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPersonalData> UserPersonalData { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users")
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserPersonalData)
                .WithOne()                          //no nav prop in UserPersonalData model
                .HasForeignKey<User>(u => u.UserPersonalDataId)
                .OnDelete(DeleteBehavior.Cascade);  //Delete personal data on User deletion

            modelBuilder.Entity<UserPersonalData>()
                .ToTable("UserPersonalData")
                .HasKey(pd => pd.Id);

            modelBuilder.Entity<UserPersonalData>()
                .ToTable("UserPersonalData")
                .Property(pd => pd.Id)
                .ValueGeneratedOnAdd();

            
        








            //Seeding initial data, move to .json, create helper service?
            if (true)
            { 
                modelBuilder.Entity<UserPersonalData>().HasData(
                    new UserPersonalData { Id = 1, Name = "Jurgis", LastName = "Jurgeliauskas", PersonalCode = "38901011234", Email = "email@email.com", PhoneNumber = "+37065123123", CityName = "Vilnius", StreetName = "Programavimo g.", HouseNumber = "1", ApartmentNumber = "1", ProfilePicture = "LocalPathURL" },
                    new UserPersonalData { Id = 2, Name = "Antanas", LastName = "Antanauskas", PersonalCode = "3702024321", Email = "antanas@email.eu", PhoneNumber = "0037065321222", CityName = "Kaunas", StreetName = "Laisves al.", HouseNumber = "5", ApartmentNumber = "2", ProfilePicture = "LocalPathURL" },
                    new UserPersonalData { Id = 3, Name = "Martyna", LastName = "Paparte", PersonalCode = "49103047474", Email = "martyna@email.co.uk", PhoneNumber = "004706532122211", CityName = "London", StreetName = "Leicester str.", HouseNumber = "16", ApartmentNumber = "200", ProfilePicture = "LocalPathURL" }
                    );

                //passwords: pass123
                modelBuilder.Entity<User>().HasData(
                    new User { Id = 1, UserName = "jurginas", UserPersonalDataId = 1, PasswordHash = Encoding.UTF8.GetBytes("1DA4A272BC00C97C9CCAAF9AFD50316247E61316F0AF951EBAAEEAFAEB44558F6D49B965465ABB03FC9E450C6E698BD7F691B27EE2A80027CDD8B674B52E5CD4"), PasswordSalt = Encoding.UTF8.GetBytes("!O%C*eFKoW&k") },
                    new User { Id = 2, UserName = "antoska", UserPersonalDataId = 2, PasswordHash = Encoding.UTF8.GetBytes("C74BB31FB8AAB1342814EEFE177710EA73C06131E409C899AA6E7907EADA8788054D0AB4EEC7B4116EE818A481DB30263C8C29CC109BC871936C34F2A2815658"), PasswordSalt = Encoding.UTF8.GetBytes("oE%gLm7elPkK") },
                    new User { Id = 3, UserName = "marmar", UserPersonalDataId = 3, PasswordHash = Encoding.UTF8.GetBytes("F8B1AE6A821659D291575A8008F3D0215176AF91147664654FB05AD3A5E8AE865B3601B41C25DDB32B4D71E021230725AFBCBFA262FFC9AB03434C16C2A921BD"), PasswordSalt = Encoding.UTF8.GetBytes("ACXDfhVmd+Ad") }
                    );
            }

        }

    }
}
