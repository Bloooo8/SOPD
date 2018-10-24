using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using SOPD.Models;

[assembly: OwinStartupAttribute(typeof(SOPD.Startup))]
namespace SOPD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();

        }

        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<User>(new UserStore<User>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!RoleManager.RoleExists("Administrator"))
            {  
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                RoleManager.Create(role);                  

                var user = new User();
                user.UserName = "Piotr_Skrzypa";
                user.Email = "testowy@gmail.com";

                string userPWD = "TestoweHasło1";

                var chkUser = UserManager.Create(user, userPWD);
 
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }
  
            if (!RoleManager.RoleExists("Dyplomant"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Dyplomant";
                RoleManager.Create(role);

            }

            
            if (!RoleManager.RoleExists("Pracownik"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Pracownik";
                RoleManager.Create(role);

            }

            if (!RoleManager.RoleExists("Promotor"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Promotor";
                RoleManager.Create(role);

            }

            if (!RoleManager.RoleExists("Recenzent"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Recenzent";
                RoleManager.Create(role);

            }

            if (!RoleManager.RoleExists("Dziekan"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Dziekan";
                RoleManager.Create(role);

            }
        }
    }
}
