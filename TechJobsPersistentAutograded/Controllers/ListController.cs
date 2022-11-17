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
    public class ListController : Controller
    {
        internal static Dictionary<string, string> ColumnChoices = new Dictionary<string, string>()
        {
            {"all", "All"},
            {"employer", "Employer"},
            {"skill", "Skill"}
        };

        internal static List<string> TableChoices = new List<string>()
        {
            "employer",
            "skill"
        };

        private JobRepository _repo;

        public ListController(JobRepository repo)
        {
            _repo = repo;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.columns = ColumnChoices;
            ViewBag.tablechoices = TableChoices;
            ViewBag.employers = _repo.GetAllEmployers();
            ViewBag.skills = _repo.GetAllSkills();
            return View();
        }

        // list jobs by column and value
        public IActionResult Jobs(string column, string value)
        {
            List<Job> jobs = new List<Job>();
            List<JobDetailViewModel> displayJobs = new List<JobDetailViewModel>();

            if (column.ToLower().Equals("all"))
            {
                jobs = _repo.GetAllJobsEmployer();

                foreach (var job in jobs)
                {
                    List<JobSkill> jobSkills = _repo.FindSkillsForJob(job.Id).ToList();

                    JobDetailViewModel newDisplayJob = new JobDetailViewModel(job, jobSkills);
                    displayJobs.Add(newDisplayJob);
                }

                ViewBag.title = "All Jobs";
            }
            else
            {
                if (column == "employer")
                {
                    jobs = _repo.FindJobsByEmployer(value).ToList();

                    foreach (Job job in jobs)
                    {
                        List<JobSkill> jobSkills = _repo.FindSkillsForJob(job.Id).ToList();

                        JobDetailViewModel newDisplayJob = new JobDetailViewModel(job, jobSkills);
                        displayJobs.Add(newDisplayJob);
                    }
                }
                else if (column == "skill")
                {
                    List<JobSkill> jobSkills = _repo.FindJobSkillsBySkill(value).ToList();

                    foreach (var job in jobSkills)
                    {
                        Job foundJob = _repo.FindJobByJobSkill(job.JobId);

                        List<JobSkill> displaySkills = _repo.FindSkillsForJob(foundJob.Id).ToList();

                        JobDetailViewModel newDisplayJob = new JobDetailViewModel(foundJob, displaySkills);
                        displayJobs.Add(newDisplayJob);
                    }
                }
                ViewBag.title = "Jobs with " + ColumnChoices[column] + ": " + value;
            }
            ViewBag.jobs = displayJobs;

            return View();
        }


    }
}
