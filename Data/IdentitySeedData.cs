using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CourseEvaluationSystem.Data
{
    public static class IdentitySeedData
    {
        private const string AdminEmail = "admin@demo.se";
        private const string AdminPassword = "Admin123!";

        private const string StudentEmail = "student@demo.se";
        private const string StudentPassword = "Student123!";

        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            // Skapa roller om de inte finns
            string[] roles = { "Admin", "Student" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Skapa admin-användare
            if (await userManager.FindByEmailAsync(AdminEmail) == null)
            {
                var admin = new IdentityUser { UserName = AdminEmail, Email = AdminEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(admin, AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Skapa student-användare
            if (await userManager.FindByEmailAsync(StudentEmail) == null)
            {
                var student = new IdentityUser { UserName = StudentEmail, Email = StudentEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(student, StudentPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(student, "Student");
                }
            }
        }
    }
}
