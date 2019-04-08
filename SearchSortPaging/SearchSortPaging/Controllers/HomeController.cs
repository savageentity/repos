using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SearchSortPaging.Models;
using PagedList;

namespace SearchSortPaging.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        StudentsDbContext db = new StudentsDbContext();
        public ActionResult Index(string option, string search, int? pageNumber, string sort)
        {

            //if the sort parameter is null or empty then we are initializing the value as descending name  
            ViewBag.SortByName = string.IsNullOrEmpty(sort) ? "descending name" : "";
            //if the sort value is gender then we are initializing the value as descending gender  
            ViewBag.SortByGender = sort == "Gender" ? "descending gender" : "Gender";

            //here we are converting the db.Students to AsQueryable so that we can invoke all the extension methods on variable records.  
            var records = db.Students.AsQueryable();

            if (option == "Subjects")
            {
                records = records.Where(x => x.Subjects == search || search == null);
            }
            else if (option == "Gender")
            {
                records = records.Where(x => x.Gender == search || search == null);
            }
            else
            {
                records = records.Where(x => x.Name.StartsWith(search) || search == null);
            }

            switch (sort)
            {

                case "descending name":
                    records = records.OrderByDescending(x => x.Name);
                    break;

                case "descending gender":
                    records = records.OrderByDescending(x => x.Gender);
                    break;

                case "Gender":
                    records = records.OrderBy(x => x.Gender);
                    break;

                default:
                    records = records.OrderBy(x => x.Name);
                    break;

            }
            return View(records.ToPagedList(pageNumber?? 1, 3));
        }
    }
}
