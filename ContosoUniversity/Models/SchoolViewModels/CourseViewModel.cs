﻿using System.Collections.Generic;

namespace ContosoUniversity.Models.SchoolViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public IEnumerable<EnrollmentViewModel> Enrollments { get; set; }
        public DepartmentViewModel Department { get; set; }
    }
}
