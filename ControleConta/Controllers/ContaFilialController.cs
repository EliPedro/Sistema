using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BancoSA.Models;
using ControleConta.Models;
using ControleConta.Helpers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace ControleConta.Controllers
{
    public class ContaFilialController : Controller
    {
        private BancoContext db = new BancoContext();
        public static BancoContext Context { get; set; }


        public async Task<ActionResult> Index()
        {
            List<ContaFilial> model = null;
            var client = new HttpClient();

            var task = await client.GetAsync("http://localhost:6060/api/ContaFilial");
            var jsonString = await task.Content.ReadAsStringAsync();
            model = JsonConvert.DeserializeObject<List<ContaFilial>>(jsonString);

            return View(model.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContaFilial contaFilial = db.ContaFilial.Find(id);
            if (contaFilial == null)
            {
                return HttpNotFound();
            }
            return View(contaFilial);
        }

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContaFilial contaFilial)
        {
            

            var resul = (from c in db.ContaFilial
                        where c.Nome.Equals(contaFilial.Nome)
                        select c.id).ToList();

          
            if(resul.Count != 0)
            {
                ModelState.AddModelError("Nome","Conta já Cadastrada");
                return View(contaFilial);
            }

            if (ModelState.IsValid)
            {                
                var conta = new Conta { Tipo = "Filial".ToUpper() };

                contaFilial.Status = "Ativo".ToUpper();
                contaFilial.Nome.ToUpper();
                contaFilial.Conta = conta;

                contaFilial.DataCriacao = DateTime.Now.Date;
               
                getContext().Conta.Add(conta);

                Context = getContext();

                Help.ContaFilial(contaFilial);
                
                return RedirectToAction("Escolha","Pessoa");
            }

            return View(contaFilial);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContaFilial contaFilial = db.ContaFilial.Find(id);
            if (contaFilial == null)
            {
                return HttpNotFound();
            }
            return View(contaFilial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ContaFilial contaFilial)
        {
            if (ModelState.IsValid)
            {

                try
                {

                    ContaFilial conta = db.ContaFilial.Find(contaFilial.id);

                    conta.Status = contaFilial.Status;
                    conta.Nome = contaFilial.Nome;


                db.Entry(conta).State = EntityState.Modified;
                db.SaveChanges();


                }catch(Exception e)
                {
                    Console.Write(e.ToString());

                }
                return RedirectToAction("Index");
            }
            return View(contaFilial);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        public  BancoContext getContext()
        {
            if (Session["Db"] == null)
            {
                Session["Db"] = new BancoContext();
            }

            return Session["Db"] as BancoContext;
        }
        
    }
}
