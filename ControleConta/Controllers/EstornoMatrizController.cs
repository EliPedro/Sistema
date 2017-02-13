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
    public class EstornoMatrizController : Controller
    {


        // GET: EstornoMatriz
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

                    var conta = (from c in db.ContaMatriz
                                 where c.Nome.Equals(estorno.ContaFavorecido)
                                 select c.Nome).ToList();

                    
                    if (conta.Count == 0)
                    {
                        ModelState.AddModelError("ContaFavorecido", "Conta Inexistente");

                        return View(estorno);
                    }


                    var token = (from c in db.Aporte
                                 where c.Token.Equals(estorno.Token)
                                 select c.Token).ToList();

                    
                    if (token.Count == 0)
                    {
                        ModelState.AddModelError("Agencia", "Código Incorreto");

                        return View(estorno);
                    }

                    

                    var v = decimal.Parse(estorno.Valor);

                    var valor = (from c in db.Aporte
                                 where c.Valor.Equals(v)
                                 select c.Valor).ToList();

                    
                    var tk = token[0];

                    if (valor.Count == 0)
                    {
                        ModelState.AddModelError("Agencia", "Informe o valor exato");

                        return View(estorno);
                    }



                    if (ModelState.IsValid)
                    {


                        var ax = (from c in db.ContaMatriz
                                  where c.Nome.Equals(estorno.ContaFavorecido)
                                  select c.id).ToList();

                        var _ax = ax.Count;


                        ContaMatriz cm = db.ContaMatriz.Find(_ax);

                        cm.Saldo -= decimal.Parse(estorno.Valor);


                        var id = (from c in db.Aporte
                                  where c.Token.Equals(estorno.Token)
                                  select c.Id).ToList();

                        int _id = id[0];

                        Aporte ap = db.Aporte.Find(_id);

                        Historico h = new Historico();
                        
                        h.Cedente = "Banco";
                        h.Sacado = estorno.ContaFavorecido;
                        h.Tipo = "Estorno".ToUpper();
                        h.Valor = decimal.Parse(estorno.Valor);
                        h.Data = DateTime.Now.Date;

                        db.Historico.Add(h);


                        db.Aporte.Remove(ap);

                        db.Entry(cm).State = EntityState.Modified;

                        db.SaveChanges();

                        return RedirectToAction("Index", "ContaMatriz");
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