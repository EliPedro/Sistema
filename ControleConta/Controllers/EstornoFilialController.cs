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
    public class EstornoFilialController : Controller
    {
        // GET: EstornoFilial
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }



        public ActionResult Create(Estorno estorno)
        {

            using (BancoContext db = new BancoContext())
            {

                try
                {

                    var conta = (from c in db.Aporte
                                 where c.Agencia.Equals(estorno.ContaFavorecido)
                                 select c.Agencia).ToList();

                    estorno.Token = null;

                    
                    if (conta.Count == 0)
                    {
                        ModelState.AddModelError("ContaFavorecido", "Conta Inexistente");

                        return View(estorno);
                    }

                    var v = decimal.Parse(estorno.Valor);

                    var valor = (from c in db.Aporte
                                 where c.Valor.Equals(v)
                                 select c.Valor).ToList();

                   


                    if (valor.Count == 0)
                    {
                        ModelState.AddModelError("Valor", "Informe o valor exato");

                        return View(estorno);
                    }


                    if (ModelState.IsValid)
                    {

                        var ax = (from c in db.ContaFilial
                                  where c.Nome.Equals(estorno.ContaFavorecido)
                                  select c.id).ToList();


                        int _ax = ax[0];

                        ContaFilial cf = db.ContaFilial.Find(_ax);

                        cf.Saldo -= decimal.Parse(estorno.Valor);
                        
                        db.Entry(cf).State = EntityState.Modified;

                        var cx = (from c in db.Aporte
                                  where c.Agencia.Equals(estorno.ContaFavorecido)
                                  select c.Id).ToList();

                        int _cx = cx[0];


                        Historico h = new Historico();

                        h.Cedente = "Banco";
                        h.Sacado = estorno.ContaFavorecido;
                        h.Tipo = "Estorno".ToUpper();
                        h.Valor = decimal.Parse(estorno.Valor);
                        h.Data = DateTime.Now.Date;
                        
                        db.Historico.Add(h);

                        Aporte ap = db.Aporte.Find(_cx);

                        db.Aporte.Remove(ap);

                        db.SaveChanges();

                        return RedirectToAction("Index", "ContaFilial");

                    }


                }
                catch (Exception e)
                {

                    Console.Write(e.ToString());
                }
            }

            return View(estorno);
        }

    }
}