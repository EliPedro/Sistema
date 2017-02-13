﻿using BancoSA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ControleConta.Service
{
    public class PessoaJuridicaController : ApiController
    {
        private BancoContext db = new BancoContext();

        public IQueryable<PessoaJuridica> GetFisica()
        {
            return db.PessoaJuridica;
        }
    }
}
