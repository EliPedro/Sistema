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
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace ControleConta.Controllers
{
    public class PessoaJuridicaController : Controller
    {
        private BancoContext db = new BancoContext();

        // GET: PessoaJuridica
        public async Task<ActionResult> Index()
        {
            List<PessoaJuridica> model = null;
            var client = new HttpClient();

            var task = await client.GetAsync("http://localhost:6060/api/PessoaJuridica");
            var jsonString = await task.Content.ReadAsStringAsync();
            model = JsonConvert.DeserializeObject<List<PessoaJuridica>>(jsonString);

            return View(model.ToList());
        }

        // GET: PessoaJuridica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PessoaJuridica pessoaJuridica = db.PessoaJuridica.Find(id);
            if (pessoaJuridica == null)
            {
                return HttpNotFound();
            }
            return View(pessoaJuridica);
        }

        // GET: PessoaJuridica/Create
        public ActionResult Create()
        {
            ViewBag.id = new SelectList(db.Pessoa, "PessoaId", "Tipo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PessoaJuridica pessoaJuridica)
        {
            if (ModelState.IsValid)
            {
                BancoContext ctx = ContaMatrizController.Context;

                try
                {
                    var resul = (from c in db.PessoaJuridica
                                 where c.CNPJ.Equals(pessoaJuridica.CNPJ)
                                 select c.CNPJ).ToList();

                    
                    if (resul.Count != 0)
                    {
                        ModelState.AddModelError("CNPJ", "CNPJ já cadastrado");
                        return View(pessoaJuridica);
                    }
                    

                    if (ctx != null)
                    {
                        var pessoa = new Pessoa { Tipo = "Juridica".ToUpper() };

                        var matriz = Help.ContaMatrix();

                        matriz.Pessoa = pessoa;
                        pessoaJuridica.Pessoa = pessoa;
                        pessoaJuridica.NomeFantasia.ToUpper();
                        pessoaJuridica.RazaoSocial.ToUpper();

                        ctx.Pessoa.Add(pessoa);
                        ctx.ContaMatriz.Add(matriz);
                        ctx.PessoaJuridica.Add(pessoaJuridica);

                        ctx.SaveChanges();

                        ContaMatrizController.Context = null;

                        TempData["msg"] = "Cadastro Efetuado com Sucessso!";

                    }
                    else
                    {


                        BancoContext db = ContaFilialController.Context;

                        var filial = Help.ContaFilial();

                        var pessoa = new Pessoa { Tipo = "Fisica" };

                        filial.Pessoa = pessoa;
                        pessoaJuridica.Pessoa = pessoa;

                        db.Pessoa.Add(pessoa);
                        db.ContaFilial.Add(filial);
                        db.PessoaJuridica.Add(pessoaJuridica);

                        db.SaveChanges();

                        TempData["msg"] = "Cadastro Efetuado com Sucessso!";
                    }
                }
                catch (Exception e)
                {
                    Response.Write(e.ToString());
                }

                return RedirectToAction("Index", "Home");

            }


            return View(pessoaJuridica);
        }

        // GET: PessoaJuridica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PessoaJuridica pessoaJuridica = db.PessoaJuridica.Find(id);
            if (pessoaJuridica == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.Pessoa, "PessoaId", "Tipo", pessoaJuridica.id);
            return View(pessoaJuridica);
        }

        // POST: PessoaJuridica/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,CNPJ,RazaoSocial,NomeFantasia")] PessoaJuridica pessoaJuridica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pessoaJuridica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.Pessoa, "PessoaId", "Tipo", pessoaJuridica.id);
            return View(pessoaJuridica);
        }

        // GET: PessoaJuridica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PessoaJuridica pessoaJuridica = db.PessoaJuridica.Find(id);
            if (pessoaJuridica == null)
            {
                return HttpNotFound();
            }
            return View(pessoaJuridica);
        }

        // POST: PessoaJuridica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
                PessoaJuridica pessoaJuridica = db.PessoaJuridica.Find(id);
                
                db.PessoaJuridica.Remove(pessoaJuridica);

                db.SaveChanges();
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
