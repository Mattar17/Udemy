﻿using System.ComponentModel.DataAnnotations;
using Udemy.Domain.Models;

namespace Udemy.Presentation.DTOs
{
    public class ChapterDTO
    {
        [Required]
        [MinLength(10)]
        public string Chapter_Name { get; set; }
        [Required]
        [Range(1,1000)]
        public int Chapter_Number { get; set; }
        [Required]
        public string Course_id { get; set; }
    }
}
