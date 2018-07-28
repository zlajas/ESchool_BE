using NLog;
using School.Models;
using School.Models.DTOs;
using School.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace School.Controllers
{
    [RoutePrefix("project/parents")]
    public class ParentsController : ApiController
    {
        private IParentsService parentsService;
        private IStudentsService studentsService;

        public ParentsController(IParentsService parentsService, IStudentsService studentsService)
        {
            this.parentsService = parentsService;
            this.studentsService = studentsService;
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Route("")]
        [Authorize(Roles = "admins")]
        public IEnumerable<Parent> GetAllParents()
        {
            logger.Info("Requesting parents");

            return parentsService.GetAllParents().OrderBy(y => y.FirstName);
        }
        [Route("{id}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Parent))]
        public IHttpActionResult GetParentById(string id)
        {         
            Parent parent = parentsService.GetById(id);

            if (parent == null)
            {
                return NotFound();
            }

            logger.Info("Requesting parent by id");

            return Ok(parent);
        }

        [Route("children/{id}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Student))]
        public IHttpActionResult GetAllParentsKids(string id)
        {          
            Parent parent = parentsService.GetById(id);

            if (parent == null)
            {
                return NotFound();
            }

            logger.Info("Requesting students linked to a parent");

            return Ok(parentsService.GetAllParentsKids(id));

        }

        [Route("remaining-students/{id}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Student))]
        public IHttpActionResult GetAllRemainingStudents(string id)
        {         
            Parent parent = parentsService.GetById(id);

            if (parent == null)
            {
                return NotFound();
            }

            logger.Info("Requesting remaining students");

            return Ok(parentsService.GetAllRemainingStudents(id));

        }

        [Route("parentsView/{id}")]
        [Authorize(Roles = "admins, parents")]
        [ResponseType(typeof(ParentViewDTO))]
        [HttpGet]
        public IHttpActionResult GetViewForParent(string id)
        {           
            Parent parent = parentsService.GetById(id);

            if (parent == null)
            {
                return NotFound();
            }

            logger.Info("Requesting marks view for parent");

            return Ok(parentsService.GetAllChildrenMarks(id));
        }

        [Route("{parentId}/addStudentToParent/{studentId}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Student))]
        [HttpPut]
        public IHttpActionResult AddStudentToParent(string parentId, string studentId)
        {         
            Parent parent = parentsService.GetById(parentId);

            if (parent == null || studentsService.GetById(studentId) == null)
            {
                return NotFound();
            }

            try
            {
                if (parent.Children.Select(x => x.Id).Contains(studentId))
                {
                    throw new Exception("Parent is already linked to this student!");
                }
                else
                {
                    logger.Info("Adding student to parent");
                    Student child = parentsService.AddStudentToParent(parentId, studentId);
                    return Ok(child);
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Adding student to parent");
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

            
        }

        [Route("{parentId}/removeStudentFromParent/{studentId}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Student))]
        [HttpPut]
        public IHttpActionResult RemoveStudentFromParent(string parentId, string studentId)
        {          
            Parent parent = parentsService.GetById(parentId);

            if (parent == null || studentsService.GetById(studentId) == null)
            {
                logger.Warn("Removing student from parent (not found in Db)");
                return NotFound();
            }

            try
            {
                if (!parent.Children.Select(x => x.Id).Contains(studentId))
                {
                    throw new NullReferenceException("Parent is not linked to this student! Nothing to remove.");
                }
                else
                {
                    logger.Info("Removing student from parent");
                    Student child = parentsService.RemoveStudentFromParent(parentId, studentId);
                    return Ok(child);
                }
            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message, "Removing subject from student");
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message));
            }           
        }

        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(Parent))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult UpdateParent(string id, Parent parent)
        {          
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Parent updatedParent = parentsService.UpdateParent(id, parent);

            if (updatedParent == null)
            {
                return NotFound();
            }

            logger.Info("Updating parent");

            return Ok(updatedParent);
        }

        [Route("{id}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Parent))]
        public IHttpActionResult DeleteParent(string id)
        {           
            Parent parent = parentsService.DeleteParent(id);

            if (parent == null)
            {
                return NotFound();
            }

            logger.Info("Deleting parent");

            return Ok(parent);
        }


    }
}
