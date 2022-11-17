using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechJobsPersistentAutograded.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [StringLength(250, MinimumLength = 3, ErrorMessage = "Description has to be between 3 and 250 characters!")]
        public string Description { get; set; }

        public List<JobSkill> JobSkills { get; set; }

        public Skill()
        {
        }

        public Skill(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
