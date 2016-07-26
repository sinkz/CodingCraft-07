using CodingCraft_07.Models;
using System;
using System.Data.Entity;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace CodingCraft_07.Controllers
{
    public class Controller<TClasse> : Controller
        where TClasse : class
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly String nomeClasse = typeof(TClasse).Name;
        private PropertyInfo propriedadeChave;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            propriedadeChave = typeof(TClasse).GetProperty(nomeClasse + "Id");
            if (!RedisCacheClient.ExistsAsync("TimeStamp" + nomeClasse).GetAwaiter().GetResult())
            {
                // TODO: Fazer a carga de dados aqui.

                RedisCacheClient.Add("TimeStamp" + nomeClasse, DateTime.Now, new TimeSpan(3, 0, 0));
            }
  
            Console.WriteLine("TimeStamp" + nomeClasse, DateTime.Now, new TimeSpan(3, 0, 0));
        }

        // GET: Empresas
        public async Task<ActionResult> Index()
        {
            var objetos = await db.Set<TClasse>()
                                        .ToListAsync();

            foreach (var objeto in objetos)
            {
                if (!await RedisCacheClient.ExistsAsync(typeof(TClasse).Name + propriedadeChave.GetValue(objeto)))
                {
                    await RedisCacheClient.AddAsync(nomeClasse + ":" + propriedadeChave.GetValue(objeto), objeto, new TimeSpan(24, 0, 0));
                }
            }

            return View(objetos);
        }

        // GET: Empresas/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var objeto = await RedisCacheClient.GetAsync<TClasse>(nomeClasse + ":" + id) ??
                              await db.Set<TClasse>().FindAsync(id);

            if (objeto == null)
            {
                return HttpNotFound();
            }

            return View(objeto);
        }

        // GET: Empresas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TClasse objeto)
        {
            if (ModelState.IsValid)
            {
                db.Set<TClasse>().Add(objeto);
                await db.SaveChangesAsync();

                await RedisCacheClient.AddAsync(nomeClasse + ":" + propriedadeChave.GetValue(objeto), objeto);

                return RedirectToAction("Index");
            }

            return View(objeto);
        }

        // GET: Empresas/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var objeto = await RedisCacheClient.GetAsync<TClasse>(nomeClasse + ":" + id) ??
                              await db.Set<TClasse>().FindAsync(id);

            if (objeto == null)
            {
                return HttpNotFound();
            }
            return View(objeto);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TClasse objeto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(objeto).State = EntityState.Modified;
                await db.SaveChangesAsync();

                await RedisCacheClient.ReplaceAsync(nomeClasse + ":" + propriedadeChave.GetValue(objeto), objeto);

                return RedirectToAction("Index");
            }
            return View(objeto);
        }

        // GET: Empresas/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var objeto = await RedisCacheClient.GetAsync<TClasse>(nomeClasse + ":" + id) ??
                              await db.Set<TClasse>().FindAsync(id);

            if (objeto == null)
            {
                return HttpNotFound();
            }
            return View(objeto);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            var objeto = await db.Set<TClasse>().FindAsync(id);
            db.Set<TClasse>().Remove(objeto);
            await db.SaveChangesAsync();

            await RedisCacheClient.RemoveAsync(nomeClasse + ":" + propriedadeChave.GetValue(objeto));

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