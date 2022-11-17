using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TechJobsPersistentAutograded.Models;

namespace TechJobsPersistentAutograded.Data
{
    public interface IJobRepository
    {
        IEnumerable<Job> GetAllJobs();
        IEnumerable<Employer> GetAllEmployers();
        IEnumerable<Skill> GetAllSkills();
        Employer FindEmployerById(int id);
        void AddNewJobSkill(JobSkill newJobSkill);
        void AddNewJob(Job newJob);
        void SaveChanges();
        Job FindJobById(int id);
        IEnumerable<JobSkill> FindSkillsForJob(int id);
        void AddNewEmployer(Employer newEmployer);
        List<Job> GetAllJobsEmployer();
        IEnumerable<Job> FindJobsByEmployer(string value);
        IEnumerable<JobSkill> FindJobSkillsBySkill(string value);
        Job FindJobByJobSkill(int id);
        void AddNewSkill(Skill newSkill);
        IEnumerable<JobSkill> FindJobsSkillsBySkillAndJob(int jobId, int skillId);
        IEnumerable<JobSkill> FindJobSkillsById(int id);
    }

    public class JobRepository : IJobRepository
    {

        private readonly JobDbContext _context; 

        public JobRepository()
        {

        }

        public JobRepository(JobDbContext context)
        {
            _context = context;
        }

        public virtual IEnumerable<Job> GetAllJobs()
        {
            return _context.Jobs.Include(j => j.Employer).Include(j => j.JobSkills).ToList();
        }

        public List<Job> GetAllJobsEmployer()
        {
            return _context.Jobs
                    .Include(j => j.Employer)
                    .ToList();
        }

        public virtual IEnumerable<Employer> GetAllEmployers()
        {
            return _context.Employers.ToList();
        }

        public virtual Employer FindEmployerById(int id)        
        {
            return _context.Employers.Find(id);
        }

        public virtual void AddNewEmployer(Employer newEmployer)       
        {
            _context.Employers.Add(newEmployer);
        }

        public virtual IEnumerable<Skill> GetAllSkills()
        {
            return _context.Skills.ToList();
        }

        public virtual void AddNewJobSkill(JobSkill newJobSkill)
        {
            _context.JobSkills.Add(newJobSkill);
        }

        public virtual void AddNewJob(Job newJob)
        {
            _context.Jobs.Add(newJob);
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }

        public virtual Job FindJobById(int id)
        {
            return _context.Jobs.Include(j => j.Employer).Single(j => j.Id == id);
        }

        public virtual IEnumerable<JobSkill> FindSkillsForJob(int id)
        {
            return _context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();
        }

        public virtual IEnumerable<Job> FindJobsByEmployer(string value)
        {
            return _context.Jobs
                        .Include(j => j.Employer)
                        .Where(j => j.Employer.Name == value)
                        .ToList();
        }

        public virtual IEnumerable<JobSkill> FindJobSkillsBySkill(string value)
        {
            return _context.JobSkills
                        .Where(j => j.Skill.Name == value)
                        .Include(j => j.Job)
                        .ToList();
        }

        public virtual Job FindJobByJobSkill(int id)
        {
            return _context.Jobs
                            .Include(j => j.Employer)
                            .Single(j => j.Id == id);
        }

        public virtual void AddNewSkill(Skill newSkill)
        {
            _context.Skills.Add(newSkill);
        }

        public virtual IEnumerable<JobSkill> FindJobsSkillsBySkillAndJob(int jobId, int skillId)
        {
            return _context.JobSkills
                    .Where(js => js.JobId == jobId)
                    .Where(js => js.SkillId == skillId)
                    .ToList();
        }

        public virtual IEnumerable<JobSkill> FindJobSkillsById(int id)
        {
            return _context.JobSkills
                .Where(js => js.SkillId == id)
                .Include(js => js.Job)
                .Include(js => js.Skill)
                .ToList();
        }
    }
}
