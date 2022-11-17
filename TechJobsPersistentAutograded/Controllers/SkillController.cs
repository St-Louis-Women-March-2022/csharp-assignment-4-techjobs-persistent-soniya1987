using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechJobsPersistentAutograded.Data;
using TechJobsPersistentAutograded.Models;
using TechJobsPersistentAutograded.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechJobsPersistentAutograded.Controllers
{
    public class SkillController : Controller
    {
        private JobRepository _repo;

        public SkillController(JobRepository repo)
        {
            _repo = repo;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Skill> skills = _repo.GetAllSkills();
            return View(skills);
        }

        public IActionResult Add()
        {
            Skill skill = new Skill();
            return View(skill);
        }

        [HttpPost]
        public IActionResult Add(Skill skill)
        {
            if (ModelState.IsValid)
            {
                _repo.AddNewSkill(skill);
                _repo.SaveChanges();
                return Redirect("/Skill/");
            }

            return View("Add", skill);
        }

        public IActionResult AddJob(int id)
        {
            Job theJob = _repo.FindJobById(id);
            IEnumerable<Skill> possibleSkills = _repo.GetAllSkills();
            AddJobSkillViewModel viewModel = new AddJobSkillViewModel(theJob, possibleSkills.ToList());
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddJob(AddJobSkillViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                int jobId = viewModel.JobId;
                int skillId = viewModel.SkillId;

                List<JobSkill> existingItems = _repo.FindJobsSkillsBySkillAndJob(jobId, skillId).ToList();

                if (existingItems.Count == 0)
                {
                    JobSkill jobSkill = new JobSkill
                    {
                        JobId = jobId,
                        SkillId = skillId
                    };

                    _repo.AddNewJobSkill(jobSkill);
                    _repo.SaveChanges();
                }

                return Redirect("/Home/Detail/" + jobId);
            }

            return View(viewModel);
        }

        public IActionResult About(int id)
        {
            IEnumerable<JobSkill> jobSkills = _repo.FindJobSkillsById(id);

            return View(jobSkills);
        }

    }
}
