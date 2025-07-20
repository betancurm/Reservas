using Microsoft.AspNetCore.Identity;
namespace apiReservas.Identity;

public static class IdentityDataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleMgr = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = { "Admin", "User" };

        foreach (var role in roles)
            if (!await roleMgr.RoleExistsAsync(role))
                await roleMgr.CreateAsync(new ApplicationRole { Name = role });

        // Usuario admin por defecto
        var adminEmail = "admin@reservas.com";
        var admin = await userMgr.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
            await userMgr.CreateAsync(admin, "Admin123$");
            await userMgr.AddToRoleAsync(admin, "Admin");
        }
    }
}
