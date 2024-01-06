using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shop_Mvc.Models;
using Shop_Mvc.Services;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;

namespace Shop_Mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDatabaseServise _DatabaseServise;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(ILogger<HomeController> logger, IDatabaseServise DatabaseServise, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _DatabaseServise = DatabaseServise;
            _memoryCache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string name)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = email, Email = email, Name = name };
                var result = await _userManager.CreateAsync(user, password);
                user = _userManager.FindByEmailAsync(email).Result;
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = Url.Action("ConfirmEmail", "User", new
                {
                    userId = user.Id,
                    token = token
                }, protocol: HttpContext.Request.Scheme);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("NOVUS", "noreplynovus547@gmail.com"));
                    message.To.Add(new MailboxAddress(name, email));
                    message.Subject = "NOVUS";
                    var htmlText = $"Дякуємо за реєстрацію, будь ласка підтвердіть ваш акаунт, перейшовши за посиланням: <a href=\"{confirmationLink}\" target=\"_blank\" rel=\"noreferrer noopener\">Підтвердити акаунт</a>.";
                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = htmlText;

                    message.Body = bodyBuilder.ToMessageBody();

                    try
                    {
                        using (var client = new MailKit.Net.Smtp.SmtpClient())
                        {
                            client.Connect("smtp.gmail.com", 587, false);
                            client.Authenticate("noreplynovus547@gmail.com", "ytufrcpfvirgxryb");
                            client.Send(message);
                            client.Disconnect(true);

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                    return Json(true);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Json(false);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(email, password, true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = _userManager.Users.FirstOrDefault(u => u.Email == email);
                    var userString = JsonConvert.SerializeObject(user);
                    Response.Cookies.Append("UserCookie", userString, new CookieOptions
                    {
                        HttpOnly = false,
                        SameSite = SameSiteMode.None,
                        Secure = Request.IsHttps,
                        Expires = DateTime.UtcNow.AddMinutes(100000000)
                    });
                    return Json(new { success = true, user });
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Json(new { success = false });
            }

            return Json(new { success = false, errorMessage = "Invalid ModelState" });
        }


        [HttpGet]
        public IActionResult IsEmailAvailable(string email) => _DatabaseServise.GetUserByEmail(email) == null ? Json(true) : Json(false);

        [HttpGet]
        public IActionResult IsEmailValid(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return Json(true);
            }
            catch (FormatException)
            {
                return Json(false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");

            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserProfileView(string email)
        {
            var userString = Request.Cookies["UserCookie"];
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<User>(userString);
                return View(user);
            }
            
            return View();
        }

        public IActionResult IsUserExist(string email, string password)
        {
            var user = _userManager.FindByEmailAsync(email).Result;

            if (user != null)
            {
                var signInResult = _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false).Result;

                if (signInResult.Succeeded)
                {
                    return Json(signInResult);
                }
            }
            return Json(false);
        }

        public IActionResult IsEmailConfirmed(string email) 
        {
            var user = _userManager?.FindByEmailAsync(email).Result;
            if (user != null)
            {
                return user.EmailConfirmed ? Json(true) : Json(false);
            }
            else return Json(false); 
        }

        [HttpGet]
        public IActionResult LogOut() 
        {
            Response.Cookies.Delete("UserCookie");
            return RedirectToAction("Index", "Home");
        }
    }
}
