﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.App_Data;
using Mhotivo.App_Data.Repositories;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class ClassActivityController : Controller
    {
        private readonly ClassActivityRepository _classActivityRepo = ClassActivityRepository.Instance;
        private readonly AcademicYearRepository _academicYearRepo = AcademicYearRepository.Instance;

        [AllowAnonymous]
        public ActionResult Index()
        {
            var message = (MessageModel)TempData["MessageInfo"];

            if (message != null)
            {
                ViewBag.MessageType = message.MessageType;
                ViewBag.MessageTitle = message.MessageTitle;
                ViewBag.MessageContent = message.MessageContent;
            }

            return View(_classActivityRepo.Query(x => x).ToList()
                .Select(x => new DisplayClassActivityModel
                {
                    AcademicYear = Convert.ToString(x.AcademicYear.Year.Year),
                    DisplayName = x.Name,
                    Type = x.Type,
                    Description = x.Description,
                    Value = Convert.ToString(x.Value),
                    ClassActivityId = x.Id
                }));   
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var thisClassActivity = _classActivityRepo.GetById(id);
            var classActivity = new ClassActivityEditModel
            {
                AcademicYearId = thisClassActivity.AcademicYear.Id,
                DisplayName = thisClassActivity.Name,
                Type = thisClassActivity.Type,
                Description = thisClassActivity.Description,
                Value = thisClassActivity.Value,
                Id = thisClassActivity.Id
            };

            ViewBag.AcademicYearId = new SelectList(_academicYearRepo.Query(x => x), "AcademicYearId", "Year", thisClassActivity.AcademicYear.Id);

            return View("Edit", classActivity);
        }

        [HttpPost]
        public ActionResult Edit(ClassActivityEditModel modelClassActivity)
        {
            var myClassActivity = _classActivityRepo.GetById(modelClassActivity.Id);
            myClassActivity.Name = modelClassActivity.DisplayName;
            myClassActivity.Type = modelClassActivity.Type;
            myClassActivity.Description = modelClassActivity.Description;
            myClassActivity.Value = modelClassActivity.Value;
            myClassActivity.AcademicYear = _classActivityRepo.GetByIdAY(modelClassActivity.AcademicYearId);

            var classactivity = _classActivityRepo.Update(myClassActivity);
            const string title = "Actividad Actualizada";
            var content = "La actividad --" + classactivity.Name + "-- ha sido actualizado exitosamente.";

            TempData["MessageInfo"] = new MessageModel
            {
                MessageType = "INFO",
                MessageTitle = title,
                MessageContent = content
            };

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            var classactivity = _classActivityRepo.Delete(id);

            const string title = "Actividad Eliminada";
            var content = "La activdad --" + classactivity.Name + "-- ha sido eliminado exitosamente.";
            TempData["MessageInfo"] = new MessageModel
            {
                MessageType = "INFO", MessageTitle = title, MessageContent = content
            };

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Add()
        {
            List<String> listTypes = new List<string>();
            listTypes.Add("Exam");
            listTypes.Add("Quiz");
            listTypes.Add("Homework");
            listTypes.Add("Classwork");
            ViewBag.Type = new SelectList(listTypes);
            ViewBag.AcademicYearId = new SelectList(_academicYearRepo.Query(x => x), "AcademicYearId", "Year");
            return View("Create");
        }

        [HttpPost]
        public ActionResult Add(ClassActivityRegisterModel modelClassActivity)
        {
            var myClassActivity = new ClassActivity
            {
                Name = modelClassActivity.DisplayName,
                Type = modelClassActivity.Type,
                Description = modelClassActivity.Description,
                Value = modelClassActivity.Value,
                AcademicYear = _classActivityRepo.GetByIdAY(modelClassActivity.AcademicYearId)
            };

            var classactivity = _classActivityRepo.Create(myClassActivity);
            const string title = "Actividad Agregada";
            var content = "La Actividad --" + classactivity.Name + "-- ha sido agregada exitosamente.";
            TempData["MessageInfo"] = new MessageModel
            {
                MessageType = "SUCCESS",
                MessageTitle = title,
                MessageContent = content
            };

            return RedirectToAction("Index");
        }
    }
}
