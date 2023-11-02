using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models;

public class Guest
{
    public int GuestId { get; set; }
    public int WeddingId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdateAt { get; set; } = DateTime.Now;
    //Propiedas de navegacion
    public User? UserGu { get; set; }
    public Wedding? WeddingGu { get; set; }

}