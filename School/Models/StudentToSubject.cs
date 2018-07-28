using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace School.Models
{
    public class StudentToSubject
    {
        [Key]
        public int StudentAttendsSubjectID { get; set; }
        [ForeignKey("Student")]
        public string StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
        [ForeignKey("Subject")]
        public int SubjectID { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        public virtual TeacherToSubject TacherTeachesSubject { get; set; }
        [JsonIgnore]
        public virtual ICollection<Mark> Marks { get; set; }

        public StudentToSubject()
        {
            Marks = new List<Mark>();
        }
    }
}