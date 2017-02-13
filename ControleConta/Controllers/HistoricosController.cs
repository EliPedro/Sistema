using BancoSA.Models;
using ControleConta.Models;
using ControleConta.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ControleConta.Controllers
{
    public class HistoricosController : Controller
    {

        public async Task<ActionResult> Index()
        {
            List<Historico> model = null;
            var client = new HttpClient();

            var task = await client.GetAsync("http://localhost:6060/api/Historico");
            var jsonString = await task.Content.ReadAsStringAsync();
            model = JsonConvert.DeserializeObject<List<Historico>>(jsonString);

            return View(model.ToList());
        }

    }
}