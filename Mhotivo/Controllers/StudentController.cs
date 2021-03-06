﻿using System;
using System.Web.Mvc;
using Mhotivo.App_Data.Repositories;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class StudentController : Controller
    {
        private readonly IContactInformationRepository _contactInformationRepository;
        private readonly IParentRepository _parentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public StudentController(IStudentRepository studentRepository, IParentRepository parentRepository,
            IContactInformationRepository contactInformationRepository)
        {
            _studentRepository = studentRepository;
            _parentRepository = parentRepository;
            _contactInformationRepository = contactInformationRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            return View(_studentRepository.GetAllStudents());
        }

        [HttpGet]
        public ActionResult ContactEdit(long id)
        {
            ContactInformation thisContactInformation = _contactInformationRepository.GetById(id);
            var contactInformation = new ContactInformationEditModel
                                     {
                                         Type = thisContactInformation.Type,
                                         Value = thisContactInformation.Value,
                                         Id = thisContactInformation.Id,
                                         People = thisContactInformation.People,
                                         Controller = "Student"
                                     };

            return View("ContactEdit", contactInformation);
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            StudentEditModel student = _studentRepository.GetStudentEditModelById(id);

            ViewBag.Tutor1Id = new SelectList(_parentRepository.Query(x => x), "Id", "FullName",
                student.FirstParent);
            ViewBag.Tutor2Id = new SelectList(_parentRepository.Query(x => x), "Id", "FullName",
                student.SecondParent);

            return View("Edit", student);
        }

        [HttpPost]
        public ActionResult Edit(StudentEditModel modelStudent)
        {
            Student myStudent = _studentRepository.GetById(modelStudent.Id);
            _studentRepository.UpdateStudentFromStudentEditModel(modelStudent, myStudent);

            const string title = "Estudiante Actualizado";
            var content = "El estudiante " + myStudent.FullName + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            Student student = _studentRepository.Delete(id);

            const string title = "Estudiante Eliminado";
            var content = "El estudiante " + student.FullName + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ContactAdd(long id)
        {
            var model = new ContactInformationRegisterModel
                        {
                            Id = (int) id,
                            Controller = "Student"
                        };
            return View("ContactAdd", model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Tutor1Id = new SelectList(_parentRepository.Query(x => x), "Id", "FullName");
            ViewBag.Tutor2Id = new SelectList(_parentRepository.Query(x => x), "Id", "FullName");
            return View("Create");
        }

        [HttpPost]
        public ActionResult Add(StudentRegisterModel modelStudent)
        {
            Student myStudent = _studentRepository.GenerateStudentFromRegisterModel(modelStudent);
            Student student = _studentRepository.Create(myStudent);
            const string title = "Estudiante Agregado";
            var content = "El estudiante " + myStudent.FullName + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(long id)
        {
            DisplayStudentModel student = _studentRepository.GetStudentDisplayModelById(id);

            return View("Details", student);
        }

        [HttpPost]
        public ActionResult Details(DisplayStudentModel modelStudent)
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DetailsEdit(long id)
        {
            StudentEditModel student = _studentRepository.GetStudentEditModelById(id);

            return View("DetailsEdit", student);
        }

        [HttpPost]
        public ActionResult DetailsEdit(StudentEditModel modelStudent)
        {
            Student myStudent = _studentRepository.GetById(modelStudent.Id);
            _studentRepository.UpdateStudentFromStudentEditModel(modelStudent, myStudent);

            const string title = "Estudiante Actualizado";
            var content = "El estudiante " + myStudent.FullName + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);

            return RedirectToAction("Details/" + modelStudent.Id);
        }
    }
}