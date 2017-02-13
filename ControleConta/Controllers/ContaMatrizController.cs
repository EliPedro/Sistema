using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BancoSA.Models;
using ControleConta.Helpers;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ControleConta.Controllers
{
    public class ContaMatrizController : Controller
    {
        private BancoContext db = new BancoContext();
        public static BancoContext Context { get; set; }

        public async Task<ActionResult> Index()
        {
            List<ContaMatriz> model = null;
            var client = new HttpClient();

            var task = await client.GetAsync("http://localhost:6060/api/ContaMatriz");
            var jsonString = await task.Content.ReadAsStringAsync();
            model = JsonConvert.DeserializeObject<List<ContaMatriz>>(jsonString);

            return View(model.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContaMatriz contaMatriz = db.ContaMatriz.Find(id);
            if (contaMatriz == null)
            {
                return HttpNotFound();
            }
            return View(contaMatriz);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContaMatriz contaMatriz)
        {
            if (ModelState.IsValid)
            {
                
                var resul = (from c in db.ContaMatriz
                             where c.Nome.Equals(contaMatriz.Nome)
                             select c.id).ToList();

                
                if (resul.Count != 0)
                {
                    ModelState.AddModelError("Nome", "Conta já Cadastrada");
                    return View(contaMatriz);
                }
                

                var conta = new Conta { Tipo = "Matriz".ToUpper() };


                contaMatriz.Nome.ToUpper();
                contaMatriz.Status = "Ativo".ToUpper();
                contaMatriz.DataCriacao = DateTime.Now.Date;
                contaMatriz.Conta = conta;

                getContext().Conta.Add(conta);

                Context = getContext();

                Help.ContaMatrix(contaMatriz);
               
                return RedirectToAction("Escolha", "Pessoa");
            }

            return View(contaMatriz);
        }

        // GET: ContaMatriz/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContaMatriz contaMatriz = db.ContaMatriz.Find(id);
            if (contaMatriz == null)
            {
                return HttpNotFound();
            }
            return View(contaMatriz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Nome,DataCriacao,Saldo,Status")] ContaMatriz contaMatriz)
        {
            if (ModelState.IsValid)
            {
                ContaMatriz conta = db.ContaMatriz.Find(contaMatriz.id);

                conta.Status = contaMatriz.Status;
                conta.Nome = contaMatriz.Nome;


                db.Entry(conta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contaMatriz);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public BancoContext getContext()
        {
            if (Session["Db"] == null)
            {
                Session["Db"] = new BancoContext();
            }

            return Session["Db"] as BancoContext;
        }

    }
}
