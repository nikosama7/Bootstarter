using Bootstarter.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bootstarter.Startup))]
namespace Bootstarter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
            //app.MapSignalR();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "ithanos@hotmail.com";
                user.Email = "ithanos@hotmail.com";
                user.CreditCard = new CreditCard()
                {
                    CVV = 666,
                    CardNumber = "1234567890000000",
                    CardOwner = "athankats",
                    Year = 2023,
                    Month = Month.December,
                    CardType = CardType.MasterCard
                };
                string userPWD = "12qwER#$";

                var chkUser = UserManager.Create(user, userPWD);

                ////Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }

                user = new ApplicationUser();
                user.UserName = "nikosama@gmail.com";
                user.Email = "nikosama@gmail.com";
                user.CreditCard = new CreditCard()
                { CVV = 123,
                  CardNumber = "1234123412345678",
                  CardOwner = "nikosama",
                  Year = 2020,
                  Month = Month.December,
                  CardType = CardType.Visa
                   
                };

                userPWD = "12qwER#$";

                chkUser = UserManager.Create(user, userPWD);

                ////Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
                
                user = new ApplicationUser();
                user.UserName = "blah@bleh.bloh";
                user.Email = "blah@bleh.bloh";

                userPWD = "23weRT$%"; user.CreditCard = new CreditCard()
                {
                    CVV = 587,
                    CardNumber = "1234567890123456",
                    CardOwner = "blah",
                    Year = 2023,
                    Month = Month.November,
                    CardType = CardType.MasterCard

                };

                chkUser = UserManager.Create(user, userPWD);

                ////Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Founder"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Founder";
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Donator"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Donator";
                roleManager.Create(role);

            }
        }
    }
}
