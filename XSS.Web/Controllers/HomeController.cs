﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Encodings.Web;
using XSS.Web.Models;

namespace XSS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly JavaScriptEncoder _javascriptEncoder;
        private readonly UrlEncoder _urlEncoder;
        public HomeController(ILogger<HomeController> logger, HtmlEncoder htmlEncoder, JavaScriptEncoder javascriptEncoder, UrlEncoder urlEncoder)
        {
            _logger = logger;
            _htmlEncoder = htmlEncoder;
            _javascriptEncoder = javascriptEncoder;
            _urlEncoder = urlEncoder;
        }

        public IActionResult Login(string returnUrl="/")
        {
            TempData["returnUrl"] = returnUrl;

            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            string returnUrl = TempData["returnUrl"].ToString();

            //email-password kontrolü

            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return Redirect("/");
        }
        public IActionResult CommentAdd()
        {
            HttpContext.Response.Cookies.Append("email", "batuhan.bedir@hotmail.com");
            HttpContext.Response.Cookies.Append("password", "123");

            if (System.IO.File.Exists("comment.txt"))
            {
                ViewBag.comments = System.IO.File.ReadAllLines("comment.txt");
            }

            return View();
        }
        //[ValidateAntiForgeryToken]
        //[IgnoreAntiforgeryToken]
        [HttpPost]
        public IActionResult CommentAdd(string name, string comment)
        {

            string encodeName = _urlEncoder.Encode(name);
            ViewBag.name = name;
            ViewBag.comment = comment;

            System.IO.File.AppendAllText("comment.txt", $"{name}-{comment}\n");

            return RedirectToAction("CommentAdd");
        }
        public IActionResult Index()
        {
            return View();
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
}