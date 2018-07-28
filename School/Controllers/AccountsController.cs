using Microsoft.AspNet.Identity;
using School.Models.DTOs;
using School.Services;
using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;

namespace School.Controllers
{
    [Authorize(Roles = "admins")]
    [RoutePrefix("project/accounts")]
    public class AccountsController : ApiController
    {
        private IAccountsService service;
        private IStudentsService studentsService;
        private IParentsService parentsService;
        private ITeachersService teachersService;

        public AccountsController(IAccountsService userService, IStudentsService studentsService, IParentsService parentsService, ITeachersService teachersService)
        {
            service = userService;
            this.studentsService = studentsService;
            this.parentsService = parentsService;
            this.teachersService = teachersService;

        }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Authorize(Roles = "admins")]
        [Route("register-student")]
        public async Task<IHttpActionResult> RegisterStudent(RegisterUserDTO userModel)
        {         
            if (studentsService.GetAllStudents().Select(x=>x.UserName).Contains(userModel.UserName))
            {
                logger.Error("Registering student - username already exists!");

                return BadRequest("Username already exists!");
            }
            var student = await service.RegisterStudent(userModel);

            logger.Info("Registering student");

            return Ok(student);
        }

        [Authorize(Roles = "admins")]
        [Route("register-teacher")]
        public async Task<IHttpActionResult> RegisterTeacher(RegisterTeacherDTO userModel)
        {
            if (teachersService.GetAllTeachers().Select(x => x.UserName).Contains(userModel.UserName) || teachersService.GetAllTeachers().Select(x => x.Email).Contains(userModel.EmailAddress))
            {
                logger.Error("Registering teacher - username or email already exists!");

                return BadRequest("Username or emailalready exists!");
            }

            var result = await service.RegisterTeacher(userModel);

            if (result == null)
            {
                return BadRequest(ModelState);
            }

            logger.Info("Registering teacher");

            return Ok();
        }

        [Authorize(Roles = "admins")]
        [Route("register-parent")]
        public async Task<IHttpActionResult> RegisterParent(RegisterParentDTO parentModel)
        {
            
            if (parentsService.GetAllParents().Select(x => x.UserName).Contains(parentModel.UserName) || parentsService.GetAllParents().Select(x => x.Email).Contains(parentModel.EmailAddress))
            {
                logger.Error("Registering parent - username already exists!");

                return BadRequest("Username already exists!");
            }

            var parent = await service.RegisterParent(parentModel);

            logger.Info("Registering parent");

            return Ok(parent);
        }

        [Authorize(Roles = "admins")]
        [Route("register-admin")]
        public async Task<IHttpActionResult> RegisterAdmin(RegisterUserDTO userModel)
        {
           
            var result = await service.RegisterAdmin(userModel);

            if (result == null)
            {
                return BadRequest(ModelState);
            }

            logger.Info("Registering admin");

            return Ok();
        }

        [Authorize(Roles = "admins")]
        [Route("logs")]
        public ICollection<string> GetLog()
        {
            
            return service.GetLogs();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }     
    }
}
