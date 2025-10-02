using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

// Konfiguration af DbContext
var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseMySql(
    "Server=localhost;Database=unibar;User=root;Password=N1naSQL#K0d3;",
    ServerVersion.AutoDetect("Server=localhost;Database=unibar;User=root;Password=N1naSQL#K0d3;")
);

using var context = new ApplicationDbContext(optionsBuilder.Options);

// Opdater password for en specifik bruger
string emailToUpdate = "ninhoe01@iba.dk"; // den bruger du vil opdatere
string newPassword = "Pass1";             // det nye password i klar tekst

var user = await context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == emailToUpdate.ToLower());

if (user == null)
{
    Console.WriteLine("User not found.");
}
else
{
    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
    user.PasswordHash = hashedPassword;

    await context.SaveChangesAsync();
    Console.WriteLine($"Password updated for user {user.Email}.");
}

// Hvis du vil opdatere alle brugere med samme password:
// var allUsers = await context.Users.ToListAsync();
// foreach (var u in allUsers)
// {
//     u.PasswordHash = BCrypt.Net.BCrypt.HashPassword("nytPassword123!");
// }
// await context.SaveChangesAsync();
