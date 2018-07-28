using NLog;
using School.Models;
using School.Models.Enum;
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
    [RoutePrefix("project/marks")]
    public class MarksController : ApiController
    {
        private IMarksService marksService;
        private IStudentsService studentsService;
        private ISubjectsService subjectsService;

        public MarksController(IMarksService marksService, IStudentsService studentsService, ISubjectsService subjectsService)
        {
            this.marksService = marksService;
            this.studentsService = studentsService;
            this.subjectsService = subjectsService;
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Route("")]
        [Authorize(Roles = "admins")]
        public IEnumerable<Mark> GetAllMarks()
        {
            logger.Info("Requesting marks");

            return marksService.GetAllGrades();
        }

        [Authorize(Roles = "admins, teachers")]
        [Route("{id}")]
        [ResponseType(typeof(Mark))]
        public IHttpActionResult GetById(int id)
        {
            logger.Info("Requesting mark by id");

            Mark mark = marksService.GetById(id);

            if (mark == null)
            {
                return NotFound();
            }
            return Ok(mark);
        }

        [Route("{studentId}/getMarks/{subjectId}")]
        [ResponseType(typeof(Mark))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetMarksPerSubject(string studentId, int subjectId)
        {           
            Student student = studentsService.GetById(studentId);

            if (student == null || subjectsService.GetById(subjectId) == null)
            {
                return NotFound();
            }

            try
            {
                if (!student.StudentAttendsSubject.Select(x => x.Subject.SubjectId).Contains(subjectId))
                {
                    throw new Exception("Student is not attending this subject!");
                }
                else
                {
                    logger.Info("Requesting marks per subject");

                    return Ok(marksService.GetMarksPerSubject(studentId, subjectId));
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Student is not attending this subject!");
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }         
        }

        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(Mark))]
        [Authorize(Roles = "admins, teachers")]
        public IHttpActionResult UpdateMark(int id, Mark grade)
        {           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mark updatedMark = marksService.UpdateMark(id, grade);

            if (updatedMark == null)
            {
                return NotFound();
            }

            logger.Info("Updating marks");

            return Ok(updatedMark);
        }

        [Route("{studentId}/addMarkToStudentAsAdmin/{subjectId}/{markValue}")]
        [HttpPut]
        [ResponseType(typeof(Mark))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult AddMarkAsAdmin(string studentId, int subjectId, int markValue)
        {
            Student student = studentsService.GetById(studentId);

            if (student == null || subjectsService.GetById(subjectId) == null)
            {
                return NotFound();
            }

            try
            {
                if (!student.StudentAttendsSubject.Select(x => x.Subject.SubjectId).Contains(subjectId))
                {
                    throw new Exception("Student is not attending this subject!");
                }
                else
                {
                    logger.Info("Adding mark as admin");

                    return Ok(marksService.AddMarkAsAdmin(studentId, subjectId, markValue));                   
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Student is not attending this subject!");
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }         
        }

        [Route("{id}")]
        [ResponseType(typeof(Mark))]
        [Authorize(Roles = "admins, teachers")]
        public IHttpActionResult DeleteMark(int id)
        {            
            Mark mark = marksService.DeleteGrade(id);

            if (mark == null)
            {
                return NotFound();
            }

            logger.Info("Deleting mark");

            return Ok(mark);
        }
    }
}
