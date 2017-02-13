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

namespace ControleConta.Controllers
{
    public class AporteController : Controller
    {
        private BancoContext db = new BancoContext();

        
        public ActionResult Index()
        {
            return View(db.Aporte.ToList());
        }
        
        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aporte aporte)
        {

            try
            {


                var r = (from c in db.ContaMatriz
                              where c.Nome.Equals(aporte.Agencia)
                              select c.Status).ToList();
                

                if (r.Count == 0)
                {
                    ModelState.AddModelError("Agencia", "Conta Inexistente");

                    return View(aporte);
                }


                var rx = (from c in db.ContaMatriz
                          where c.Status.Equals("Desativado".ToUpper()) || c.Status.Equals("Cancelado".ToUpper())
                          select c.Status).ToList();

            

                if (rx.Count > 0)
                {
                    ModelState.AddModelError("Agencia", "Conta Cancelada ou Bloqueada");

                    return View(aporte);
                }



                if (String.IsNullOrEmpty(aporte.Token)) 
               {
                    ModelState.AddModelError("Token", "Informe um código"); 

                return View(aporte);
            }

            if((aporte.Token.Length - 1) > 4)
                {

                    ModelState.AddModelError("Token", "Máximo 4 caracteres");

                }


                if (Help.IsValid(aporte.Token) == false)
               {

                ModelState.AddModelError("Token", "Chave Invalida");
            
                return View(aporte);
             }


            var id = (from c in db.ContaMatriz
                         where c.Nome.Equals(aporte.Agencia)
                         select c.id).ToList();
                
                   
            if(id.Count == 0)
            {
                ModelState.AddModelError("Agencia", "Conta Inexistente");

                return View(aporte);
            }


                var resul = (from c in db.Aporte
                            where c.Token.Equals(aporte.Token)
                            select c.Token).ToList();

          
            if(resul.Count != 0 )
            {
                    ModelState.AddModelError("Token", "Token já cadastrado");

                    return View(aporte);
            }

            if (ModelState.IsValid)
            {


                 var contaId = (from c in db.ContaMatriz
                                 where c.Nome.Equals(aporte.Conta)
                                 select c.id).ToList();


                var _contaId = id.Count;
                    
                ContaMatriz m = db.ContaMatriz.Find(id[0]);
                m.Saldo += aporte.Valor;
                db.Entry(m).State = EntityState.Modified;

                aporte.Agencia.ToString().ToUpper();
                aporte.Token.ToUpper();


                Historico h = new Historico();

                h.Tipo = "Aporte".ToUpper();
                h.Data = DateTime.Now.Date;
                h.Cedente = aporte.Agencia;
                h.Sacado = m.Nome;
                h.Valor = aporte.Valor;

                db.Historico.Add(h);
                db.Aporte.Add(aporte);
                db.SaveChanges();

                return RedirectToAction("Index","ContaMatriz");
            }
            }
            catch (Exception e)
            {

                Console.Write(e.ToString());
            }

            return View(aporte);
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
