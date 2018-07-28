using NLog;
using School.Models;
using School.Models.DTOs;
using School.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace School.Controllers
{
    [RoutePrefix("project/students")]
    public class StudentsController : ApiController
    {
        private IStudentsService studentsService;
        private IMarksService marksService;
        private ISubjectsService subjectsService;
        private ITeachersService teachersService;

        public StudentsController(IStudentsService studentsService, IMarksService marksService, ISubjectsService subjectsService, ITeachersService teachersService)
        {
            this.studentsService = studentsService;
            this.marksService = marksService;
            this.subjectsService = subjectsService;
            this.teachersService = teachersService;

        }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Authorize(Roles = "admins")]
        [Route("")]
        public IEnumerable<Student> GetAllStudents()
        {
            logger.Info("Requesting students");

            return studentsService.GetAllStudents().OrderBy(y=>y.FirstName);
        }
        [Route("{id}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Student))]
        public IHttpActionResult GetStudentById(string id)
        {           
            Student student = studentsService.GetById(id);
            
            if (student==null)
            {
                return NotFound();
            }

            logger.Info("Requesting student by id");

            return Ok(student);
        }

        [Route("subjects/{id}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Subject))]
        public IHttpActionResult GetAllStudentsSubjects(string id)
        {           
            Student student = studentsService.GetById(id);

            if (student == null)
            {
                return NotFound();
            }

            logger.Info("Requesting subjects for a student");

            return Ok(studentsService.GetAllStudentsSubjects(id));

        }

        [Route("remaining-subjects/{id}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Subject))]
        public IHttpActionResult GetAllRemainingSubjects(string id)
        {           
            Student student = studentsService.GetById(id);

            if (student == null)
            {
                return NotFound();
            }

            logger.Info("Requesting remaining subjects");

            return Ok(studentsService.GetAllRemainingSubjects(id));

        }

        [Route("marks/{id}")]
        [Authorize(Roles = "admins, students")]
        [ResponseType(typeof(IEnumerable<StudentViewDTO>))]
        public IHttpActionResult GetAllMarksForStudent(string id)
        {         
            Student student = studentsService.GetById(id);

            if (student == null)
            {
                return NotFound();
            }

            logger.Info("Requesting all marks - student view");

            return Ok(studentsService.GetAllMarks(id));
        }

        [Route("{studentId}/getMarks/{subjectId}")]
        [ResponseType(typeof(Mark))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetMarksPerSubject(string studentId, int subjectId)
        {          
            Student student = studentsService.GetById(studentId);

            if (student == null)
            {
                return NotFound();
            }

            logger.Info("Requesting marks per subject");

            return Ok(marksService.GetMarksPerSubject(studentId, subjectId));
        }

        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(Student))]
        [Authorize(Roles = "admins")]
        public IHttpActionResult UpdateStudent(string id, Student student)
        {           
            Student updatedStudent = studentsService.UpdateStudent(id, student);

            if(updatedStudent == null)
            {
                return NotFound();
            }

            logger.Info("Updating student");

            return Ok(updatedStudent);
        }
        [Route("{studentId}/addSubjectToStudent/{subjectId}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Student))]
        [HttpPut]
        public IHttpActionResult AddSubjectToStudent(string studentId, int subjectId)
        {           
            Student student = studentsService.GetById(studentId);

            if (student == null || subjectsService.GetById(subjectId) == null)
            {
                return NotFound();
            }

            try
            {
                if (student.StudentAttendsSubject.Select(x => x.Subject.SubjectId).Contains(subjectId))
                {
                    throw new Exception("Student already attends this subject!");                   
                }
                else
                {
                    logger.Info("Adding subject to student");

                    studentsService.AddSubjectToStudent(studentId, subjectId);
                    return Ok(student.StudentAttendsSubject.Select(x => x.Subject));
                }
            }
            catch(Exception e)
            {
                logger.Error(e, "Subject already added");
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("{studentId}/and/{subjectId}/addTeacherToStudentSubject/{teacherId}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Student))]
        [HttpPut]
        public IHttpActionResult AddTeacherToStudentSubject(string studentId, int subjectId, string teacherId)
        {           
            Student student = studentsService.GetById(studentId);
            Teacher teacher = teachersService.GetById(teacherId);

            if (student == null || teacher == null || subjectsService.GetById(subjectId) == null)
            {
                return NotFound();
            }

            try
            {
                if (!student.StudentAttendsSubject.Select(x => x.Subject.SubjectId).Contains(subjectId))
                {
                    throw new NullReferenceException("Student doesn't attend this subject!");
                }
                else if (!teacher.TeacherTeachesSubject.Select(x => x.Subject?.SubjectId).Contains(subjectId))
                {
                    throw new NullReferenceException("Teacher doesn't teach this subject and therefore cannot be added to this student-subject pair!");
                }
                else
                {
                    logger.Info("Linking teacher with student-subject");

                    studentsService.AddTeacherToStudentSubject(studentId, subjectId, teacherId);
                    return Ok(student.StudentAttendsSubject.Select(x => x.TacherTeachesSubject?.Teacher));
                }

            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message, "Adding teacher to student-subject pair");
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message));
            }
          
        }            

        [Route("{studentId}/deleteSubjectFromStudent/{subjectId}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(StudentToSubject))]
        [HttpPut]
        public IHttpActionResult RemoveSubjectFromStudent(string studentId, int subjectId)
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
                    throw new NullReferenceException("Student is not attending this subject! Nothing to remove.");
                }
                else
                {
                    logger.Info("Removing subject from student");

                    StudentToSubject studentSubject = studentsService.RemoveSubjectStudentPair(studentId, subjectId);
                    return Ok(studentSubject);
                }

            }
            catch (NullReferenceException e)
            {
                logger.Error(e.Message, "Removing subject from student");
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message));
            }          
        }


        [Route("specialAddSubjectsToStudent")] //special method to link subjects to students by birth date. Used only for testing purposes.
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(void))]
        [HttpPut]
        public void SpecialAddSubjectsToStudent()
        {
            logger.Info("Adding subjects to students by birth date");

            studentsService.SpecialAddSubjectToStudent();
        }

        [Route("{id}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(Student))]
        public IHttpActionResult DeleteStudent(string id)
        {          
            Student student = studentsService.DeleteStudent(id);

            if(student == null)
            {
                return NotFound();
            }

            logger.Info("Deleting student");

            return Ok(student);
        }




    }
}
