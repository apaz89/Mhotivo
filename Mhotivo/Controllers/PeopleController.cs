﻿using System.Web.Mvc;
using Mhotivo.App_Data.Repositories;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public PeopleController(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            return View(_peopleRepository.GetAllPeople());
        }

        [HttpGet]
        public ActionResult Edit(long Id)
        {
            PeopleEditModel people = _peopleRepository.GetPeopleEditModelById(Id);

            return View("Edit", people);
        }

        [HttpPost]
        public ActionResult Edit(PeopleEditModel peopleModel)
        {
            People people = _peopleRepository.GetById(peopleModel.Id);
            _peopleRepository.UpdatePeopleFromPeopleEditModel(peopleModel, people);

            const string title = "Persona Actualizada";
            var content = "La persona " + people.FullName + " - " + people.Id + " ha sido actualizada exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);

            return RedirectToAction("Index");
        }
    }
}