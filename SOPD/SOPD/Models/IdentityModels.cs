using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SOPD.Models
{
    // Możesz dodać dane profilu dla użytkownika, dodając więcej właściwości do klasy ApplicationUser. Odwiedź stronę https://go.microsoft.com/fwlink/?LinkID=317594, aby dowiedzieć się więcej.
    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public override string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Adres e-mail")]
        public override string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Numer telefonu")]
        public override string PhoneNumber { get; set; }

        [ForeignKey("OrganizationalUnit")]
        public int? OrganizationalUnitID { get; set; }

        public string FullName {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public virtual OrganizationalUnit OrganizationalUnit { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Element authenticationType musi pasować do elementu zdefiniowanego w elemencie CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Dodaj tutaj niestandardowe oświadczenia użytkownika
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {     
        }
        public DbSet<Thesis> Theses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<OrganizationalUnit> OrganizationalUnits { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}