using JulioLoyalty.Business.Parameters;
using JulioLoyalty.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JulioLoyalty.UI.Controllers.WebAPI
{
    [RoutePrefix("api/update")]
    public class UpdateController : ApiController
    {
        [HttpPut]
        [Route("EditRoles")]
        public IHttpActionResult EditRoles(RequestRol reques)
        {
            IRepositoryService service = new DefaultService();
            return Json(service.UpdateRol(reques));
        }
        [HttpPut]
        [Route("AddRol")]
        public IHttpActionResult AddRol(RequestRol reques)
        {
            IRepositoryService service = new DefaultService();
            return Json(service.CreateRol(reques));
        }

        [HttpGet]
        [Route("EditUserAccess")]
        public IHttpActionResult EditUserAccess(string key, string arrayKey)
        {
            IRepositoryService service = new DefaultService();
            return Json(service.UpdateUserAccess(key, arrayKey));
        }

        [HttpPost]
        [Route("EditUser")]
        public IHttpActionResult EditUser(RequesUser request)
        {
            IRepositoryService service = new DefaultService();
            return Json(service.UpdateUser(request));
        }
        [HttpPost]
        [Route("CreateUser")]
        public IHttpActionResult CreateUser(RequesUser request)
        {
            IRepositoryService service = new DefaultService();
            return Json(service.CreateUser(request));
        }

        [HttpPost]
        [Route("AddOrUpdateAction")]
        public IHttpActionResult addOrUpdateAction(RequestAction request)
        {
            IRepositoryService service = new DefaultService();
            return Json(service.AddOrUpdateAction(request));
        }

        [HttpGet]
        [Route("DeleteAction")]
        public IHttpActionResult DeleteAction(int Id)
        {
            IRepositoryService service = new DefaultService();
            return Json(service.DeleteAction(Id));
        }



        [HttpGet]
        [Route("Actualizar/Lista/Distribuidor")]
        public IHttpActionResult ULDistribuidor(string key, string arrayKey)
        {
            IRepositoryService service = new DefaultService();
            return Json(service.ALDistribuidor(key, arrayKey));
        }
    }
}
