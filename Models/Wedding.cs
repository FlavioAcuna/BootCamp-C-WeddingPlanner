#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using DateValidator.Models;

namespace WeddingPlanner.Models;
public class Wedding
{
    [Key]
    public int WeddingId { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    [MinLength(2, ErrorMessage = "Debe tener minimo 2 caracteres")]
    public string WedderOne { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    [MinLength(2, ErrorMessage = "Debe tener minimo 2 caracteres")]
    public string WedderTwo { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    [DataType(DataType.DateTime)]
    [FutureDate]
    public DateTime WeddingDate { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    public string address { get; set; }

    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdateAt { get; set; } = DateTime.Now;
    public List<Guest> GuestWedding { get; set; } = new List<Guest>();
}