using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace School.Models
{
    public class Student : ApplicationUser
    {
        [Required]
        public DateTime DateOfBirth { get; set; }
        [JsonIgnore]
        public virtual ICollection<StudentToSubject> StudentAttendsSubject { get; set; }
        [JsonIgnore]
        public virtual ICollection<Parent> Parents { get; set; }

        public Student()
        {
            StudentAttendsSubject = new List<StudentToSubject>();
            Parents = new List<Parent>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}