using AgeRanger.Models;
using DataLayer.Repository;
using System.Collections.Generic;
using System.Web.Mvc;
using Domain = DataLayer.Domain;

namespace AgeRanger.Controllers
{    
    public class HomeController : Controller
    {
        private List<AgeGroup> ListAgeGroup
        {
            get
            {
                var result = PersonRepository.GetAgeGroup();

                var _listAgeGroup = new List<AgeGroup>();

                foreach (var item in result)
                {
                    var ageGroup = new AgeGroup
                    {
                        Id = item.Id,
                        MinAge = item.MinAge == null ? 0 : item.MinAge,
                        MaxAge = item.MaxAge == null ? 0 : item.MaxAge,
                        Description = item.Description
                    };

                    _listAgeGroup.Add(ageGroup);
                }

                return _listAgeGroup;
            }
        }

        private SqLitePersonRepository PersonRepository
        {
            get
            {
                return new SqLitePersonRepository();
            }
        }        

        public ActionResult Index()
        {
            SqLitePersonRepository testing = new SqLitePersonRepository();

            var testing1 = testing.GetAllPerson();

            return View();
        }

        public ActionResult SaveDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveDetails(Person personModel)
        {
            Domain.Person personDomain = new Domain.Person
            {
                FirstName = personModel.FirstName,
                LastName = personModel.LastName,
                Age = personModel.Age
            };

            var testing1 = PersonRepository.SavePerson(personDomain);

            return RedirectToAction("GetDetails");
        }


        public ActionResult GetDetails()
        {
            var result = PersonRepository.GetAllPerson();

            var personList = new List<Person>();

            foreach (var item in result)
            {
                var tempAge = item.Age;

                var person = new Person
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Age = item.Age,
                    Description = ListAgeGroup.Find(x => tempAge >= x.MinAge && (x.MaxAge != 0 ? tempAge < x.MaxAge : true)).Description
                };

                personList.Add(person);
            }

            return View(personList);
        }

        public ActionResult FindDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FindDetails(Models.Person personModel)
        {
            var results = PersonRepository.SearchPerson(personModel.FirstName, personModel.LastName);

            var personList = new List<Person>();

            foreach (var item in results)
            {
                var tempAge = item.Age;

                var person = new Person
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Age = item.Age,
                    Description = ListAgeGroup.Find(x => tempAge >= x.MinAge && tempAge < x.MaxAge).Description
                };

                personList.Add(person);
            }

            ViewBag.PersonDetails = personList;

            return View();
        }

        public ActionResult Details(string id)
        {
            long _id = 0;

            long.TryParse(id, out _id);

            var result = PersonRepository.GetPerson(_id);

            Person personDomain = new Person
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                Age = result.Age,
                Description = result.Description                
            };

            return View(personDomain);
        }        
    }
}
