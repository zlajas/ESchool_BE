using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using School.Models.Enum;

namespace School.Models
{
    public class Subject
    {
        [Key]
        public int? SubjectId { get; set; }
        [Required]
        public string SubjectName { get; set; }
        [Required]
        [Range(1, 6, ErrorMessage = ("Weekly fund must have a value between 1 and 6"))]
        public int WeeklyFund { get; set; }
        [Required]
        public ESemester Semester { get; set; }
        [Required]
        public EGradeYear Grade { get; set; }
        [JsonIgnore]
        public virtual ICollection<StudentToSubject> StudentAttendsSubjects { get; set; }
        [JsonIgnore]
        public virtual ICollection<TeacherToSubject> TeacherTeachesSubjects { get; set; }
       
        public Subject()
        {
            StudentAttendsSubjects = new List<StudentToSubject>();
            TeacherTeachesSubjects = new List<TeacherToSubject>();
        }

    }
}