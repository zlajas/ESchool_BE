using Microsoft.AspNet.Identity;
using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Repositories
{
    public interface IAuthRepository : IDisposable
    {
        Task<Student> RegisterStudent(Student student, string password);
        Task<Parent> RegisterParent(Parent parent, string password);
        Task<IdentityResult> RegisterTeacher(Teacher teacher, string password);
        Task<IdentityResult> RegisterAdmin(Admin admin, string password);
        Task<ApplicationUser> FindUser(string userName, string password);
        Task<IList<string>> FindRoles(string userId);
    }


}
