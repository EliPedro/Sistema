using BancoSA.Models;
using ControleConta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ControleConta.Service
{
    public class ContaFilialController : ApiController
    {
        private BancoContext db = new BancoContext();

        public IQueryable<ContaFilial> GetFilial()
        {
            return db.ContaFilial;
        }
    }
}
