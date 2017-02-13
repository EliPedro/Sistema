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
    public class PessoaFisicasController : Controller
    {
        private BancoContext db = new BancoContext();


        // GET: PessoaFisicas
        public async Task<ActionResult> Index()
        {
            List<PessoaFisica> model = null;
            var client = new HttpClient();

            var task = await client.GetAsync("http://localhost:6060/api/PessoaFisica");
            var jsonString = await task.Content.ReadAsStringAsync();
            model = JsonConvert.DeserializeObject<List<PessoaFisica>>(jsonString);

            return View(model.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PessoaFisica pessoaFisica = db.PessoaFisica.Find(id);
            if (pessoaFisica == null)
            {
                return HttpNotFound();
            }
            return View(pessoaFisica);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PessoaFisica pessoaFisica)
        {



            var resul = (from c in db.PessoaFisica
                         where c.CPF.Equals(pessoaFisica.CPF)
                         select c.CPF).ToList();

            
            if (resul.Count != 0)
            {
                ModelState.AddModelError("CPF", "CPF já cadastrado");
                return View(pessoaFisica);
            }




            if (Help.IsValidCPF(pessoaFisica.CPF) == false)
            {
                ModelState.AddModelError("CPF", "CPF incorreto");
           }
            if (ModelState.IsValid)
            {
                BancoContext ctx  = ContaMatrizController.Context;

                try { 

                if (ctx != null)
                {
                    var pessoa = new Pessoa { Tipo = "Fisica".ToUpper() };
                    
                    var matriz = Help.ContaMatrix();

                    matriz.Pessoa = pessoa;
                    pessoaFisica.Pessoa = pessoa;
                    pessoaFisica.Nome.ToUpper();
                    ctx.Pessoa.Add(pessoa);
                    ctx.ContaMatriz.Add(matriz);
                    ctx.PessoaFisica.Add(pessoaFisica);

                    ctx.SaveChanges();

                    ContaMatrizController.Context = null;

                    TempData["msg"] = "Cadastro Efetuado com Sucessso!";

                    }
                    else
                   {


                    BancoContext db = ContaFilialController.Context;

                    var filial = Help.ContaFilial();
                        
                    var pessoa = new Pessoa { Tipo = "Fisica".ToUpper() };
                   
                    filial.Pessoa = pessoa;
                    pessoaFisica.Pessoa = pessoa;
                    pessoaFisica.Nome.ToUpper();

                    db.Pessoa.Add(pessoa);
                    db.ContaFilial.Add(filial);
                    db.PessoaFisica.Add(pessoaFisica);
                    
                    db.SaveChanges();

                    TempData["msg"] = "Cadastro Efetuado com Sucessso!";

                    }
                }
                catch(Exception e)
                {
                    Response.Write(e.ToString());
                }

                return RedirectToAction("Index", "Home");

            }

            return View(pessoaFisica);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PessoaFisica pessoaFisica = db.PessoaFisica.Find(id);
            if (pessoaFisica == null)
            {
                return HttpNotFound();
            }
            return View(pessoaFisica);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CPF,Nome,DataNascimento")] PessoaFisica pessoaFisica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pessoaFisica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pessoaFisica);
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
