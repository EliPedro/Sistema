using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleConta.Controllers
{
    public class AgenciaController : Controller
    {
        // GET: Agencia
        public ActionResult Escolha()
        {
            return View();
        }
    }
}