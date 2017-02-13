using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BancoSA.Models;
using ControleConta.Models;

namespace ControleConta.Service
{
    public class HistoricoController : ApiController
    {
        private BancoContext db = new BancoContext();

        // GET: api/Historicos
        public IQueryable<Historico> GetHistorico()
        {
            return db.Historico;
        }

        // GET: api/Historicos/5
        [ResponseType(typeof(Historico))]
        public IHttpActionResult GetHistorico(int id)
        {
            Historico historico = db.Historico.Find(id);
            if (historico == null)
            {
                return NotFound();
            }

            return Ok(historico);
        }

        // PUT: api/Historicos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHistorico(int id, Historico historico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != historico.Id)
            {
                return BadRequest();
            }

            db.Entry(historico).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoricoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Historicos
        [ResponseType(typeof(Historico))]
        public IHttpActionResult PostHistorico(Historico historico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Historico.Add(historico);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = historico.Id }, historico);
        }

        // DELETE: api/Historicos/5
        [ResponseType(typeof(Historico))]
        public IHttpActionResult DeleteHistorico(int id)
        {
            Historico historico = db.Historico.Find(id);
            if (historico == null)
            {
                return NotFound();
            }

            db.Historico.Remove(historico);
            db.SaveChanges();

            return Ok(historico);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HistoricoExists(int id)
        {
            return db.Historico.Count(e => e.Id == id) > 0;
        }
    }
}