#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    [MinLength(2, ErrorMessage = "El nombre de tener al menos 2 caracteres")]
    public string Nombre { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    [MinLength(2, ErrorMessage = "El apellido de tener al menos 2 caracteres")]
    public string Apellido { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    [EmailAddress]
    [EmailUnico]
    public string Email { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "La contraseña de contener al menos 8 caracteres")]
    public string Password { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    [DataType(DataType.Password)]
    [NotMapped]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdateAt { get; set; } = DateTime.Now;

    public List<Guest> GuestUser { get; set; } = new List<Guest>();
}