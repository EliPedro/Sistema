using BancoSA.Models;
using ControleConta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleConta.Controllers
{
    public class TransferenciaController : Controller
    {
        private ContaFilial Favorecido { get; set; }
        private ContaFilial Cedente { get; set; }
        private int Chave { get; set; }
        private int _Chave { get; set; }
        private decimal Saldo { get; set; }
        

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(Transferencia transferencia)
        {
            if (ModelState.IsValid)
            {

                try
                {

                    using (BancoContext db = new BancoContext())
                    {

                        if (transferencia.Conta.Equals(transferencia.ContaFavorecido))
                        {

                            ModelState.AddModelError("ContaFavorecido", "*");
                            return View(transferencia);
                        }


                        var chave = (from n in db.ContaFilial
                                     where n.Nome.Equals(transferencia.Conta)
                                     select n.id).ToList();

                        if (chave.Count == 0)
                        {
                            ModelState.AddModelError("Conta", "Conta Inexistente");

                            return View(transferencia);
                        }




                        var id = (from n in db.ContaFilial
                                  where n.Nome.Equals(transferencia.ContaFavorecido)
                                  select n.id).ToList();

                        if (id.Count == 0)
                        {
                            ModelState.AddModelError("ContaFavorecido", "Conta Inexistente");
                            return View(transferencia);
                        }


                        var status = (from n in db.ContaFilial
                                      where n.Nome.Equals(transferencia.Conta)
                                      select n.Status).ToList();

                        var _status = (from n in db.ContaFilial
                                       where n.Nome.Equals(transferencia.ContaFavorecido)
                                       select n.Status).ToList();

                        var _Tipo = (from n in db.ContaFilial
                                     where n.Nome.Equals(transferencia.ContaFavorecido)
                                     select n.Conta.Tipo).ToList();


                        var result = (from n in db.ContaFilial
                                      where n.Nome.Equals(transferencia.ContaFavorecido)
                                      select n.Status).ToList();

    

                                if (status[0] == "Ativo".ToUpper())
                                {

                                    if (_Tipo[0] != "Matriz".ToUpper())
                                    {
  
                                        var saldo = (from n in db.ContaFilial
                                                     where n.Nome.Equals(transferencia.Conta)
                                                     select n.Saldo).ToList();

                                        Chave = id[0];
                                        Saldo = saldo[0];
                                        _Chave = chave[0];
                                    }
                                }
                                
                        }

                }
                

                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }


                if(ModelState.IsValid)
                { 
                using (BancoContext db = new BancoContext())
                {
                        if (Saldo > decimal.Parse(transferencia.Valor))
                        {
                            Favorecido = db.ContaFilial.Find(Chave);

                            Favorecido.Saldo += decimal.Parse(transferencia.Valor);


                            Cedente = db.ContaFilial.Find(_Chave);

                            Cedente.Saldo -= decimal.Parse(transferencia.Valor);


                            Historico h = new Historico();

                            h.Tipo = "Transferência".ToUpper();
                            h.Data = DateTime.Now.Date;
                            h.Cedente = transferencia.Conta;
                            h.Sacado = transferencia.ContaFavorecido;
                            h.Valor = decimal.Parse(transferencia.Valor);

                            db.Historico.Add(h);

                            db.Entry(Favorecido).State = EntityState.Modified;
                            db.Entry(Cedente).State = EntityState.Modified;

                            db.SaveChanges();


                            TempData["msg"] = "Transferência realizada com sucesso";

                            return RedirectToAction("Index", "Home");
                        }
                    }

                }
            }

            return View();
        }  
       
        }
    }
