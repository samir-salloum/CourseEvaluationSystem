﻿using System;

namespace CourseEvaluationSystem.Models.ViewModels
{
    public class CourseEvaluationItemVm
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
