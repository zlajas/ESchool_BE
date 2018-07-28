using Microsoft.AspNet.Identity;
using School.Models;
using School.Models.Enum;
using School.Models.DTOs;
using School.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace School.Services
{
    public class AccountsService : IAccountsService
    {
        private IUnitOfWork db;

        public AccountsService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public async Task<IdentityResult> RegisterAdmin(RegisterUserDTO userModel)
        {
            Admin user = new Admin
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
            };

            return await db.AuthRepository.RegisterAdmin(user, userModel.Password);
        }

        public async Task<Student> RegisterStudent(RegisterUserDTO userModel)
        {
            Student user = new Student
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                DateOfBirth = userModel.DateOfBirth
            };

            return await db.AuthRepository.RegisterStudent(user, userModel.Password);

        }

        public async Task<Parent> RegisterParent(RegisterParentDTO parentModel)
        {
            Parent parent = new Parent
            {
                UserName = parentModel.UserName,
                FirstName = parentModel.FirstName,
                LastName = parentModel.LastName,
                Email = parentModel.EmailAddress
            };

            return await db.AuthRepository.RegisterParent(parent, parentModel.Password);
        }

        public async Task<IdentityResult> RegisterTeacher(RegisterTeacherDTO userModel)
        {
            Teacher user = new Teacher
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.EmailAddress
            };

            return await db.AuthRepository.RegisterTeacher(user, userModel.Password);
        }

        public ICollection<string> GetLogs()
        {
            StreamReader sr;
            string fileLocation = @"C:\Users\Zlatko Spasojević\Desktop\project back\School\logs\app-log.txt";
            List<string> logs = new List<string>();

            try
            {
                sr = new StreamReader(fileLocation);
                while (true)
                {
                    string line = sr.ReadLine();
                    logs.Add(line);
                    if (string.IsNullOrEmpty(line))
                    {
                        break;
                    }
                }
                logs.Reverse();
                return logs;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }

}   