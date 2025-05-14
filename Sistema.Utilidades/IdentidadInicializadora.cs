using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Sistema.Models;


namespace Sistema.Utilidades
{
    public static class IdentidadInicializadora
    {
        public static async Task CrearRolesYSuperUsuarioAsync(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Usamos las constantes CNT en lugar de strings sueltos
                string[] roles = { CNT.Admin, CNT.Almacen, CNT.Venta };

                foreach (var rol in roles)
                {
                    if (!await roleManager.RoleExistsAsync(rol))
                    {
                        await roleManager.CreateAsync(new IdentityRole(rol));
                    }
                }

                // Crear súper usuario admin si no existe
                string emailAdmin = "superAdmin@gmail.com";
                string passwordAdmin = "12345678Dd$";

                if (await userManager.FindByEmailAsync(emailAdmin) == null)
                {
                    var superUsuario = new ApplicationUser
                    {
                        UserName = emailAdmin,
                        Email = emailAdmin,
                        Nombre = "Super Administrador",
                        EmailConfirmed = true
                    };

                    var resultado = await userManager.CreateAsync(superUsuario, passwordAdmin);

                    if (resultado.Succeeded)
                    {
                        await userManager.AddToRoleAsync(superUsuario, CNT.Admin);
                    }
                    else
                    {
                        foreach (var error in resultado.Errors)
                        {
                            Console.WriteLine($"Error creando superusuario: {error.Description}");
                        }
                    }
                }
            }
        }
    }
}
