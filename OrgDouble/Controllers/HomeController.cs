using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrgDouble.Models;
using OrgDouble.Models.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OrgDouble.Controllers
{
    public class HomeController : Controller
    {
        /* private readonly ILogger<HomeController> _logger;

         public HomeController(ILogger<HomeController> logger)
         {
             _logger = logger;
         }*/
        private IRepository _repo;
        public HomeController(IRepository repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Success()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(IdForBase idForBase)
        {
            if (ModelState.IsValid)
            {

                _repo.SetAllPersonBase(idForBase.IdTo, idForBase.IdFrom);
            }

            return Redirect("~/Home/Success");
        }

        public IActionResult ClearPersons()
        {
            _repo.Сlear();
            return Redirect("~/Home/Privacy");
        }
        [HttpGet]

        public IActionResult Delite()
        {
            List<Person> source = _repo.SelectAll().ToList();
            List<Person> del = new List<Person>();
            foreach (Person p in source)
            {
                if (p.Done)

                    del.Add(p);
            }
            _repo.DelitePersons(del);
            return Redirect("~/Home/Privacy");
        }
        public IActionResult Privacy(int page=1)
        {
            int pageSize = 25;
           List<Person> source = _repo.SelectAll().ToList();
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            PrivacyViewModel privacyViewModel = new PrivacyViewModel
            { PageViewModel = pageViewModel,
              Persons = items

            };



            return View(privacyViewModel);
        }
        [HttpPost]
        public IActionResult Privacy(string[] items)
        {
            int count = items.Length;
            for (int i = 0; i < count; i++)
            {
                _repo.SetCheck(Int32.Parse(items[i]));
            }

            return Redirect("~/Home/Privacy");
           /* return View(_repo.SelectAll());*/
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
