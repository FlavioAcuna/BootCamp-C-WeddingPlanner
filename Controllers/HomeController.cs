#pragma warning disable CS8629
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost("user/create")]
    public IActionResult RegisterUser(User newUser)
    {
        if (ModelState.IsValid)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);
            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            return RedirectToAction("DashboardWedding");
        }
        else
        {
            return View("Index");
        }
    }
    [HttpPost("")]
    public IActionResult ValidaUser(LoginUser userLogin)
    {
        if (ModelState.IsValid)
        {
            User? UserExist = _context.users.FirstOrDefault(u => u.Email == userLogin.EmailLogin);
            if (UserExist == null)
            {
                ModelState.AddModelError("EmailLogin", "Correo o contraseña invalidos");
                return View("Index");
            }
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            var result = hasher.VerifyHashedPassword(userLogin, UserExist.Password, userLogin.PasswordLogin);
            if (result == 0)
            {
                ModelState.AddModelError("EmailLogin", "Correo o contraseña invalidos");
                return View("Index");
            }
            HttpContext.Session.SetInt32("UserId", UserExist.UserId);
            return RedirectToAction("DashboardWedding");
        }
        else
        {
            return View("Index");
        }
    }


    [SessionCheck]
    [HttpGet("weddings")]
    public IActionResult DashboardWedding()
    {
        int? UserId = HttpContext.Session.GetInt32("UserId");
        ViewBag.SelUser = _context.users.FirstOrDefault(u => u.UserId == UserId);
        List<Wedding> weddins = _context.weddings.
        Include(w => w.GuestWedding).
        ThenInclude(u => u.UserGu).
        ToList();
        return View(weddins);
    }
    [SessionCheck]
    [HttpGet("weddings/new")]
    public IActionResult AddWedding()
    {
        int? UserId = HttpContext.Session.GetInt32("UserId");
        ViewBag.SelUser = _context.users.FirstOrDefault(u => u.UserId == UserId);
        return View("AddWedding");
    }

    [SessionCheck]
    [HttpPost("weddings/new")]
    public IActionResult CreateWedding(Wedding newWedding, int WedId)
    {
        int? UserId = HttpContext.Session.GetInt32("UserId");
        int ussid = (int)UserId;
        ViewBag.SelUser = _context.users.FirstOrDefault(u => u.UserId == UserId);
        if (ModelState.IsValid)
        {
            newWedding.UserId = ussid;
            _context.weddings.Add(newWedding);
            _context.SaveChanges();
            WedId = _context.weddings.Where(u => u.UserId == ussid).OrderByDescending(w => w.WeddingId).Select(w => w.WeddingId).FirstOrDefault();
            Guest? addfirstguest = new Guest();
            addfirstguest.WeddingId = WedId;
            addfirstguest.UserId = (int)UserId;
            _context.guests.Add(addfirstguest);
            _context.SaveChanges();
            Console.WriteLine($"---------------------------");
            Console.WriteLine($"Weddding id: {WedId}");
            Console.WriteLine($"---------------------------");
            return View("WeddingView", newWedding);
        }
        return View("AddWedding");
    }

    [SessionCheck]
    [HttpGet("weddings/{WedId}")]
    public IActionResult ViewWedding(int WedId)
    {
        int? UserId = HttpContext.Session.GetInt32("UserId");
        int ussid = (int)UserId;
        ViewBag.SelUser = _context.users.FirstOrDefault(u => u.UserId == UserId);
        Wedding? weddins = _context.weddings.
            Include(w => w.GuestWedding).
            ThenInclude(u => u.UserGu).
            FirstOrDefault(e => e.WeddingId == WedId);
        return View("WeddingView", weddins);
    }

    [SessionCheck]
    [HttpPost("weddings/{WedId}/destroy")]
    public IActionResult DeleteWedding(int WedId)
    {
        int? UserId = HttpContext.Session.GetInt32("UserId");
        ViewBag.SelUser = _context.users.FirstOrDefault(u => u.UserId == UserId);
        List<Wedding> weddins = _context.weddings.
        Include(w => w.GuestWedding).
        ThenInclude(u => u.UserGu).
        ToList();
        Wedding? selectWedding = _context.weddings.SingleOrDefault(wed => wed.WeddingId == WedId);
        _context.weddings.Remove(selectWedding);
        _context.SaveChanges();
        return RedirectToAction("DashboardWedding", weddins);
    }
    [SessionCheck]
    [HttpPost("weddings/{WedId}/noasistir")]
    public IActionResult noAsistir(int WedId)
    {
        int? UserId = HttpContext.Session.GetInt32("UserId");
        ViewBag.SelUser = _context.users.FirstOrDefault(u => u.UserId == UserId);
        List<Wedding> weddins = _context.weddings.
        Include(w => w.GuestWedding).
        ThenInclude(u => u.UserGu).
        ToList();
        Guest? selectWedding = _context.guests.FirstOrDefault(g => g.UserId == UserId && g.WeddingId == WedId);
        _context.guests.Remove(selectWedding);
        _context.SaveChanges();
        return RedirectToAction("DashboardWedding", weddins);
    }
    [SessionCheck]
    [HttpPost("weddings/{WedId}/asistir")]
    public IActionResult AsistirWed(int WedId)
    {
        int? UserId = HttpContext.Session.GetInt32("UserId");
        ViewBag.SelUser = _context.users.FirstOrDefault(u => u.UserId == UserId);
        List<Wedding> weddins = _context.weddings.
        Include(w => w.GuestWedding).
        ThenInclude(u => u.UserGu).
        ToList();
        Guest? newGuest = new Guest();
        newGuest.UserId = (int)UserId;
        newGuest.WeddingId = (int)WedId;
        _context.guests.Add(newGuest);
        _context.SaveChanges();
        return RedirectToAction("DashboardWedding", weddins);
    }



    [HttpGet("")]
    public IActionResult LogOut()
    {
        HttpContext.Session.SetString("UserId", "");
        HttpContext.Session.Clear();
        return View("index");
    }






    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //Encontrar la sesion 
        int? UserId = context.HttpContext.Session.GetInt32("UserId");
        if (UserId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}