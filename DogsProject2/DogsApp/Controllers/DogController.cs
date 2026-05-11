using DogsApp.Ifrastructure.Data;
using DogsApp.Ifrastructure.Data.Domain;
using DogsApp.Models.Dog;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogsApp.Controllers
{
    public class DogController : Controller
    {
        private readonly ApplicationDbContext _Context;

        public DogController(ApplicationDbContext context)
        {
            _Context = context;
        }

        // GET: DogController
        public ActionResult Index(string searchStringBreed,string searchStringName)
        {
            List<DogAllViewModel> dogs = _Context.Dogs
                .Select(DogFromDb => new DogAllViewModel
                {
                    Id = DogFromDb.Id,
                    Name = DogFromDb.Name,
                    Age = DogFromDb.Age,
                    Breed = DogFromDb.Breed,
                    Picture = DogFromDb.Picture,
                }).ToList();
            if(!string.IsNullOrEmpty(searchStringBreed) && !string.IsNullOrEmpty(searchStringName) )
            {
                dogs = dogs.Where(d => d.Breed.Contains(searchStringBreed) && d.Name.Contains(searchStringName)).ToList();
            }
            else if(!string.IsNullOrEmpty(searchStringBreed))
            {
                dogs = dogs.Where( d => d.Breed.Contains(searchStringBreed)).ToList();

            }
            else if(!string.IsNullOrEmpty(searchStringName))
            {
                dogs = dogs.Where(d => d.Name.Contains(searchStringName)).ToList();
            }

                return View(dogs);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            Dog? item = _Context.Dogs.Find(id);
            if(item == null)
            {
                return NotFound();
            }
            DogDetailsViewModel dog = new DogDetailsViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                Breed = item.Breed,
                Picture = item.Picture
            };
            return View(dog);

        }

        // GET: DogController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DogCreateViewModel bindingModel)
        {
            if(ModelState.IsValid)
            {
                Dog dogFromDb = new Dog
                {
                    Name = bindingModel.Name,
                    Age = bindingModel.Age,
                    Breed = bindingModel.Breed,
                    Picture = bindingModel.Picture,
                };
                _Context.Dogs.Add(dogFromDb);
                _Context.SaveChanges();

                return this.RedirectToAction("Success");

            }
            return this.View();
        }
        public IActionResult Success()
        {
            return this.View();
        }


        // GET: DogController/Edit/5
        public ActionResult Edit(int? id)
        {
           if(id == null)
            {
                return NotFound();
            }
            Dog? item = _Context.Dogs.Find(id);
            if(item == null)
            {
                return NotFound();
            }
            DogEditViewModel dog = new DogEditViewModel()
            {
                Id = item.Id,
                Name = item.Name,

                Age = item.Age,
                Breed = item.Breed,
                Picture = item.Picture
            };
            return View(dog);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DogEditViewModel bindingModel)
        {
           if(ModelState.IsValid)
            {
                Dog dog = new Dog
                {
                    Id = id,
                    Name = bindingModel.Name,
                    Age = bindingModel.Age,
                    Breed = bindingModel.Breed,
                    Picture = bindingModel.Picture
                };  
                _Context.Dogs.Update(dog);
                _Context.SaveChanges();
                return this.RedirectToAction("Index");
            }
           return View(bindingModel);
        }

        // GET: DogController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Dog? item = _Context.Dogs.Find(id);
            if(item == null)
            {
                return NotFound();
            }
            DogDetailsViewModel dog = new DogDetailsViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                Breed = item.Breed,
                Picture = item.Picture
            };
            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            Dog? item = _Context.Dogs.Find(id);

            if(item == null)
            {
                return NotFound();
            }
            _Context.Dogs.Remove(item);
            _Context.SaveChanges();
            return this.RedirectToAction("Index", "Dog");
        }
    }
}
