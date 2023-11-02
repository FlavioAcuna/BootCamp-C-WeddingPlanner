namespace WeddingPlanner.Models;

public class UsGuWeModel
{
    public User? newUser { get; set; } = new User();
    public LoginUser? ValUser { get; set; } = new LoginUser();

    public List<Wedding> weddins { get; set; } = new List<Wedding>();
    public int Guescount { get; set; }
}