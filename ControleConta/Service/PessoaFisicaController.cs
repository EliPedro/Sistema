using BancoSA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ControleConta.Service
{
    public class PessoaFisicaController : ApiController
    {
        private BancoContext db = new BancoContext();

        // GET: PessoaFisica
        public IQueryable<PessoaFisica> GetFisica()
        {
            return db.PessoaFisica;
        }
    }
}