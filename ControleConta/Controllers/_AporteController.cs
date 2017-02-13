using BancoSA.Models;
using ControleConta.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleConta.Controllers
{
    public class _AporteController : Controller
    {
        private BancoContext db = new BancoContext();

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]    
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Aporte aporte)
        {


            var result = (from c in db.ContaFilial
                          where c.Nome.Equals(aporte.Agencia)
                          select c.id).ToList();

            if (result.Count == 0)
            {
                ModelState.AddModelError("Agencia", "Conta Inexistente");

                return View(aporte);
            }

            var rx = (from c in db.ContaFilial
                      where c.Status.Equals("Desativado".ToUpper()) || c.Status.Equals("Cancelado".ToUpper())
                      select c.Status).ToList();
            
            if ( rx.Count > 0)
            {
                ModelState.AddModelError("Agencia", "Conta Cancelada ou Bloqueada");
                return View(aporte);
            }
            
         


            if (ModelState.IsValid)
            {

                try
                { 

                var id = (from c in db.ContaFilial
                          where c.Nome.Equals(aporte.Agencia)
                          select c.id).ToList();


                ContaFilial f = db.ContaFilial.Find(id[0]);
                f.Saldo += aporte.Valor;
                db.Entry(f).State = EntityState.Modified;

                aporte.Agencia.ToString().ToUpper();
                aporte.Token = "";

                db.Aporte.Add(aporte);

                Historico h = new Historico();
     
                h.Tipo = "Aporte".ToUpper();
                h.Data = DateTime.Now.Date;
                h.Cedente = aporte.Agencia;
                h.Sacado = f.Nome;
                h.Valor = aporte.Valor;   
               
                db.Historico.Add(h);
                db.SaveChanges();

                return RedirectToAction("Index", "ContaFilial");

                }catch(Exception e)
                {
                    Console.Write(e.ToString());
                }
            }


            return View(aporte);
        }
    }
}