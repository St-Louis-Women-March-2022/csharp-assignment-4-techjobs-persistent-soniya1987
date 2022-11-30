using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistentAutograded.Models;
using TechJobsPersistentAutograded.ViewModels;
using TechJobsPersistentAutograded.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistentAutograded.Controllers
{

    public class HomeController : Controller

    {
        private JobRepository _repo;

        public HomeController(JobRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()

        {
            IEnumerable<Job> jobs = _repo.GetAllJobs();

            return View(jobs);
        }


        [HttpGet("/Add")]
        public IActionResult AddJob()
        {
            List<Employer> employer = _repo.GetAllEmployers().ToList();
            List<Skill> skills = _repo.GetAllSkills().ToList();
            AddJobViewModel viewModel = new AddJobViewModel(employer, skills);
            return View(viewModel);
        }

        [HttpPost()]
        public IActionResult ProcessAddJobForm(AddJobViewModel addJobViewModel, string[] selectedSkills)
        {
            if (ModelState.IsValid)
            {
                Job job = new Job()
                {
                    Name = addJobViewModel.Name,
                    EmployerId = addJobViewModel.EmployerId,
                    
                };
                foreach (var skill in selectedSkills)
                {
                    JobSkill jobSkill = new JobSkill();
                    jobSkill.Job = job;
                    jobSkill.SkillId = Convert.ToInt32(skill);
                    job.JobSkills.Add(jobSkill);
                }  
               
                _repo.AddNewJob(job);
                _repo.SaveChanges();
                return Redirect("Index");
            }

            return View("Add", addJobViewModel);
        }


        public IActionResult Detail(int id)
        {
            Job theJob = _repo.FindJobById(id);

            List<JobSkill> jobSkills = _repo.FindSkillsForJob(id).ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }

    }

}


