using System.ComponentModel.DataAnnotations;

namespace ClassLibrary1.Entity;
public class Survey
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Please give the survey a name")]
    public string? Name { get; set; }
    public string? Description { get; set; }

    // this relationship is configured as a optional, since the Survey.AdminId foreign key property is nullable, so deleting Survey will not cascade to AdminUser
    // https://docs.microsoft.com/en-us/ef/core/saving/cascade-delete#deleting-a-principalparent
    public string? AdminUserId { get; set; }
    public AdminUser? AdminUser { get; set; }

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<Participant> Participants { get; set; } = new List<Participant>();
}
