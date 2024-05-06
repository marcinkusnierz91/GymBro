using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace GymBro.Models;

public class UserModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Last_name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public DateTime Date_created { get; set; } = DateTime.Now;
    public List<TrainingModel> Trainings { get; set; }

}

public class UserModelValdator : AbstractValidator<UserModel>
{
    public UserModelValdator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Pusto tu, dej emaila")
            .EmailAddress()
            .WithMessage("Brak maila");
    }
}