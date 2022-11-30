using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechJobsPersistentAutograded.Data;
using TechJobsPersistentAutograded.Models;
using TechJobsPersistentAutograded.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechJobsPersistentAutograded.Controllers
{
    public class EmployerController : Controller
    {
        private readonly JobRepository _jobRepository;
        public EmployerController(JobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Employer> employers = _jobRepository.GetAllEmployers();
            return View(employers);
        }

        public IActionResult Add()
        {
            AddEmployerViewModel viewModel = new AddEmployerViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ProcessAddEmployerForm(AddEmployerViewModel addEmployerViewModel)
        {
            if (ModelState.IsValid)
            {
                Employer employer = new Employer();
                employer.Location = addEmployerViewModel.Location;
                employer.Name = addEmployerViewModel.Name;
                _jobRepository.AddNewEmployer(employer);
                _jobRepository.SaveChanges();
                return Redirect("/Employer");
            }
            return View("Add", addEmployerViewModel);
        }

        public IActionResult About(int id)
        {
            AddEmployerViewModel viewModel = new AddEmployerViewModel();
            Employer employer = _jobRepository.GetAllEmployers().Where(x => x.Id == id).First();
            viewModel.Name = employer.Name;
            viewModel.Location = employer.Location;
            return View(viewModel);
        }
    }
}

