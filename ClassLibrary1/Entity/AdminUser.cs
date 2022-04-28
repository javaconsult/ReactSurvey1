using Microsoft.AspNetCore.Identity; //dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
namespace ClassLibrary1.Entity;
public class AdminUser : IdentityUser
{
    public ICollection<Survey>? Surveys { get; set; }
    public string? DisplayName { get; set; }
}

