using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechJobsPersistentAutograded.Models;
using TechJobsPersistentAutograded.Data;
using Microsoft.EntityFrameworkCore;
using TechJobsPersistentAutograded.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechJobsPersistentAutograded.Controllers
{
    public class SearchController : Controller
    {
        private JobRepository _repo;

        public SearchController(JobRepository repo)
        {
            _repo = repo;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.columns = ListController.ColumnChoices;
            return View();
        }

        public IActionResult Results(string searchType, string searchTerm)
        {
            List<Job> jobs;
            List<JobDetailViewModel> displayJobs = new List<JobDetailViewModel>();

            if (string.IsNullOrEmpty(searchTerm))
            {
                jobs = _repo.GetAllJobsEmployer();

                foreach (var job in jobs)
                {
                    List<JobSkill> jobSkills = _repo.FindSkillsForJob(job.Id).ToList();

                    JobDetailViewModel newDisplayJob = new JobDetailViewModel(job, jobSkills);
                    displayJobs.Add(newDisplayJob);
                }
            }
            else
            {
                if (searchType == "employer")
                {
                    jobs = _repo.FindJobsByEmployer(searchTerm).ToList();

                    foreach (Job job in jobs)
                    {
                        List<JobSkill> jobSkills = _repo.FindSkillsForJob(job.Id).ToList();

                        JobDetailViewModel newDisplayJob = new JobDetailViewModel(job, jobSkills);
                        displayJobs.Add(newDisplayJob);
                    }

                }
                else if (searchType == "skill")
                {
                    List<JobSkill> jobSkills = _repo.FindJobSkillsBySkill(searchTerm).ToList();

                    foreach (var job in jobSkills)
                    {
                        Job foundJob = _repo.FindJobByJobSkill(job.JobId);

                        List<JobSkill> displaySkills = _repo.FindSkillsForJob(foundJob.Id).ToList();

                        JobDetailViewModel newDisplayJob = new JobDetailViewModel(foundJob, displaySkills);
                        displayJobs.Add(newDisplayJob);
                    }
                }
            }

            ViewBag.columns = ListController.ColumnChoices;
            ViewBag.title = "Jobs with " + ListController.ColumnChoices[searchType] + ": " + searchTerm;
            ViewBag.jobs = displayJobs;

            return View("Index");
        }

    }
}

