﻿using System.ComponentModel.DataAnnotations;

namespace TechJobsPersistentAutograded.ViewModels
{
    public class AddEmployerViewModel
    {
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Location is required")]
        public string Location { get; set; }
    }
}