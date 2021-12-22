using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ControleWeb2.Models;
using PagedList;

namespace ControleWeb2.Controllers
{
    public class EquipamentosController : Controller
    {
        private ControleWebEntities db = new ControleWebEntities();

        // GET: Equipamentos
        /*public async Task<ActionResult> Index()
        {
            return View(await db.Equipamentos.ToListAsync());
        }*/
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.EquipamentosSortParm = String.IsNullOrEmpty(sortOrder) ? "equipamentos_desc" : "";
            ViewBag.SerialNumberSortParm = String.IsNullOrEmpty(sortOrder) ? "serialnumber_asc" : "serialnumber_desc";
            ViewBag.SistemaAutomacaoSortParm = String.IsNullOrEmpty(sortOrder) ? "sistemaautomacao_asc" : "";
            //ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var equipamentos = from s in db.Equipamentos
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                equipamentos = equipamentos.Where(s => s.Equipamento.Contains(searchString)
                                                || s.Modelo.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "equipamentos_desc":
                    equipamentos = equipamentos.OrderByDescending(s => s.Equipamento);
                    break;
                case "serialnumber_asc":
                    equipamentos = equipamentos.OrderBy(s => s.SerialNumber);
                    break;
                case "serialnumber_desc":
                    equipamentos = equipamentos.OrderByDescending(s => s.SerialNumber);
                    break;
                case "sistemaautomacao_asc":
                    equipamentos = equipamentos.OrderBy(s => s.SistemaAutomação);
                    break;
                default:
                    equipamentos = equipamentos.OrderBy(s => s.Equipamento);
                    break;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(equipamentos.ToPagedList(pageNumber, pageSize));

            //return View(equipamentos.ToList());
        }

        // GET: Equipamentos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipamentos equipamentos = await db.Equipamentos.FindAsync(id);
            if (equipamentos == null)
            {
                return HttpNotFound();
            }
            return View(equipamentos);
        }

        // GET: Equipamentos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Equipamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Equipamento,Modelo,SerialNumber,DataCadastro,Local,Armario,Prateleira,SistOper,AplicativoInstalado,SistemaAutomação,Status,PC,RC,ItemRC,NF,DataGarantia,Observacao")] Equipamentos equipamentos)
        {
            if (ModelState.IsValid)
            {
                db.Equipamentos.Add(equipamentos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(equipamentos);
        }

        // GET: Equipamentos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipamentos equipamentos = await db.Equipamentos.FindAsync(id);
            if (equipamentos == null)
            {
                return HttpNotFound();
            }
            return View(equipamentos);
        }

        // POST: Equipamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Equipamento,Modelo,SerialNumber,DataCadastro,Local,Armario,Prateleira,SistOper,AplicativoInstalado,SistemaAutomação,Status,PC,RC,ItemRC,NF,DataGarantia,Observacao")] Equipamentos equipamentos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipamentos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(equipamentos);
        }

        // GET: Equipamentos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipamentos equipamentos = await db.Equipamentos.FindAsync(id);
            if (equipamentos == null)
            {
                return HttpNotFound();
            }
            return View(equipamentos);
        }

        // POST: Equipamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Equipamentos equipamentos = await db.Equipamentos.FindAsync(id);
            db.Equipamentos.Remove(equipamentos);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
