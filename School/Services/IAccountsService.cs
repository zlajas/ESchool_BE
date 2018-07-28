using Microsoft.AspNet.Identity;
using School.Models.DTOs;
using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Services
{
    public interface IAccountsService
    {
        Task<IdentityResult> RegisterAdmin(RegisterUserDTO user);
        Task<Student> RegisterStudent(RegisterUserDTO user);
        Task<Parent> RegisterParent(RegisterParentDTO user);
        Task<IdentityResult> RegisterTeacher(RegisterTeacherDTO user);
        ICollection<string> GetLogs();
    }
}
