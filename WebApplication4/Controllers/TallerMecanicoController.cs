using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

using System.Net.Mail;
using System.Net;



namespace WebApplication4.Controllers
{
    public class TallerMecanicoController : Controller
    {
        private TallerMecanicoEntities db = new TallerMecanicoEntities();


		[AllowAnonymous]
		public ActionResult Login()
		{
				return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(String usuario)
		{


			// asi como esta funciona, la libreria de mail no corre el connect */
			var client = new SmtpClient("smtp.gmail.com", 587)
			{
				Credentials = new NetworkCredential("leandro.fernandezp@gmail.com", "CONTRASEÑADELGMAIL"),
				EnableSsl = true
			};
			client.Send("leandro.fernandezp@gmail.com", "leandro.fernandezp@gmail.com", "test", "testbody");
			Console.WriteLine("Sent");
			//------------------------------------------------------------------



			if (usuario == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			List<Empleado> empleados = db.Empleado.ToList();
			Empleado empleado = null;

			foreach (Empleado emp in empleados)
			{
				if (emp.usuario.Equals(usuario))
				{
					empleado = emp;
				}
			}
			if (empleado == null)
			{
				ViewBag.Mensaje = "Usuario Incorrecto";
				return View("Login");   // para testing debe ser EXPLICITA
			}

			//TODO: Validate credentials Correctly, this code is only for demo !!
			bool isCredentialValid = true;
            if (isCredentialValid)
            {
                var token = TokenGenerator.GenerateTokenJwt(usuario);
				ViewBag.token = token;
				if (empleado.TipoEmpleado.nombre.Equals("empleado"))
				{
					return RedirectToAction("indexEmpleado", empleado);
				}
				if (empleado.TipoEmpleado.nombre.Equals("supervisor"))
				{
					return RedirectToAction("indexSupervisor", empleado);
				}
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
            else
            {
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}
		
		}

		// GET: TallerMecanico
		public ActionResult IndexEmpleado()
        {
            var ordenTrabajo = db.OrdenTrabajo.Include(o => o.Empleado).Include(o => o.Vehiculo);
            return View(ordenTrabajo.ToList());
        }

		public ActionResult IndexSupervisor()
		{
			var repuesto = db.Repuesto;
			return View(repuesto.ToList());
		}

		// GET: TallerMecanico/Details/5
		
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenTrabajo ordenTrabajo = db.OrdenTrabajo.Find(id);
            if (ordenTrabajo == null)
            {
                return HttpNotFound();
            }
            return View(ordenTrabajo);
        }

        // GET: TallerMecanico/Create
        public ActionResult Create()
        {
            ViewBag.idEmpleado = new SelectList(db.Empleado, "idEmpleado", "nombre");
            ViewBag.idVehiculo = new SelectList(db.Vehiculo, "idVehiculo", "marca");
			return View();
        }

        // POST: TallerMecanico/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idOrdenTrabajo,fechaIngreso,detalle,idVehiculo,idEmpleado")] OrdenTrabajo ordenTrabajo)
        {
            if (ModelState.IsValid)
            {
                db.OrdenTrabajo.Add(ordenTrabajo);
                db.SaveChanges();
                return RedirectToAction("IndexEmpleado");
            }

            ViewBag.idEmpleado = new SelectList(db.Empleado, "idEmpleado", "nombre");
            ViewBag.idVehiculo = new SelectList(db.Vehiculo, "idVehiculo", "marca");
			return View();
        }


		// GET: TallerMecanico/Edit/5
		public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenTrabajo ordenTrabajo = db.OrdenTrabajo.Find(id);
            if (ordenTrabajo == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEmpleado = new SelectList(db.Empleado, "idEmpleado", "nombre", ordenTrabajo.idEmpleado);
            ViewBag.idVehiculo = new SelectList(db.Vehiculo, "idVehiculo", "marca", ordenTrabajo.idVehiculo);
            return View(ordenTrabajo);
        }

        // POST: TallerMecanico/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idOrdenTrabajo,idEmpleado,idVehiculo,fechaIngreso,cerrada,detalle")] OrdenTrabajo ordenTrabajo)
        {

			//ordenTrabajo.cerrada = false;

			if (ModelState.IsValid)
            {
                db.Entry(ordenTrabajo).State = EntityState.Modified;
                db.SaveChanges();

				if (ordenTrabajo.cerrada == true) { 
				IEnumerable<RepuestoOrden> repuestosOrden = db.RepuestoOrden.Where(rep => rep.idOrdenTrabajo == ordenTrabajo.idOrdenTrabajo).ToList();
				if(repuestosOrden != null)
					return View("ListCost",repuestosOrden);
				}
				return RedirectToAction("IndexEmpleado");
            }
            ViewBag.idEmpleado = new SelectList(db.Empleado, "idEmpleado", "nombre", ordenTrabajo.idEmpleado);
            ViewBag.idVehiculo = new SelectList(db.Vehiculo, "idVehiculo", "marca", ordenTrabajo.idVehiculo);
            return View(ordenTrabajo);
        }

        // GET: TallerMecanico/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenTrabajo ordenTrabajo = db.OrdenTrabajo.Find(id);
            if (ordenTrabajo == null)
            {
                return HttpNotFound();
            }
            return View(ordenTrabajo);
        }

        // POST: TallerMecanico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrdenTrabajo ordenTrabajo = db.OrdenTrabajo.Find(id);
            db.OrdenTrabajo.Remove(ordenTrabajo);
            db.SaveChanges();
            return RedirectToAction("IndexEmpleado");
        }

		// SUPERVISOR

		public ActionResult CreateRepuesto()
		{
			return View();
		}

		// POST: TallerMecanico/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateRepuesto([Bind(Include = "idRepuesto,nombre,costo" )] Repuesto repuesto)
		{
			if (ModelState.IsValid)
			{
				db.Repuesto.Add(repuesto);
				db.SaveChanges();
				return RedirectToAction("IndexSupervisor");
			}

			return View();
		}

	
		public ActionResult CreateEmpleado()
		{
			ViewBag.idTipoEmpleado = new SelectList(db.TipoEmpleado , "idTipoEmpleado", "nombre");
			return View();
		}

		// POST: TallerMecanico/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
	
		public ActionResult CreateEmpleado([Bind(Include = "idEmpleado,nombre,idTipoEmpleado,usuario")] Empleado empleado)
		{
			if (ModelState.IsValid)
			{
				db.Empleado.Add(empleado);
				db.SaveChanges();
				return RedirectToAction("IndexSupervisor");
			}

			return View();
		}

		public ActionResult EditRepuesto(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Repuesto repuesto = db.Repuesto.Find(id);
			if (repuesto == null)
			{
				return HttpNotFound();
			}
	
			return View(repuesto);
		}

		// POST: TallerMecanico/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditRepuesto([Bind(Include = "idRepuesto,nombre,costo")] Repuesto repuesto)
		{
			if (ModelState.IsValid)
			{
				db.Entry(repuesto).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("IndexSupervisor");
			}
			
			return View(repuesto);
		}

		// EMPLEADO

		public ActionResult CreateClient()
		{
			return View();
		}

		// POST: TallerMecanico/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateClient([Bind(Include = "DNI , nombre , apellido , email")] Cliente cliente)
		{
			if (ModelState.IsValid)
			{
				db.Cliente.Add(cliente);
				db.SaveChanges();
				ViewBag.DNI = new SelectList(db.Cliente, "DNI", "nombre", "apellido");
				return RedirectToAction("CreateVehicle");
			}
			return View();
		}

		public ActionResult CreateVehicle()
		{
			ViewBag.DNI = new SelectList(db.Cliente, "DNI", "nombre" , "apellido");
			return View();
		}

		// POST: TallerMecanico/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateVehicle([Bind(Include = "idVehiculo , DNI, marca, patenta")] Vehiculo vehiculo)
		{
			if (ModelState.IsValid) { 
				db.Vehiculo.Add(vehiculo);
				db.SaveChanges();
				return RedirectToAction("IndexEmpleado");
			}
			ViewBag.DNI = new SelectList(db.Cliente, "DNI", "nombre", "apellido");
			return View();
		}


		public ActionResult CreateRepuestoOrden(int? id)
		{

			if(id != null)
			{
				OrdenTrabajo ordentrabajo = db.OrdenTrabajo.Find(id);
				RepuestoOrden repuestoOrden = new RepuestoOrden();
				repuestoOrden.idOrdenTrabajo = ordentrabajo.idOrdenTrabajo;
				ViewBag.idRepuesto = new SelectList(db.Repuesto, "idRepuesto", "nombre");
				return View(repuestoOrden);
			}
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			/* 
			<div class="form-group">
				@Html.LabelFor(model => model.idOrdenTrabajo, "idOrdenTrabajo", htmlAttributes: new { @class = "control-label col-md-2" })
				<div class="col-md-10">
					@Html.DropDownList("idOrdenTrabajo", null, htmlAttributes: new { @class = "form-control" })
					@Html.ValidationMessageFor(model => model.idOrdenTrabajo, "", new { @class = "text-danger" })
				</div>
			</div>
			*/
		}

		// POST: TallerMecanico/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateRepuestoOrden([Bind(Include = "idOrdenTrabajo,idRepuesto,cantidadHs,cantidadRepuesto")] RepuestoOrden repuestoOrden)
		{
			if (ModelState.IsValid)
			{
				db.RepuestoOrden.Add(repuestoOrden);
				db.SaveChanges();
				return RedirectToAction("IndexEmpleado");
			}

			ViewBag.idRepuesto = new SelectList(db.Repuesto, "idRepuesto", "nombre");
			return View(repuestoOrden);

		}


		public ActionResult ListCost( int? id)
		{

			// TESTEAR DE CERRAR LA QUE TIENE 2 REPUESTOS A VER SI TRAE BIEN REPUESTO ORDEN Y SI CALCULA BIEN PRECIOS

			if(id != null)
			{
				IEnumerable<RepuestoOrden> repuestosOrden = db.RepuestoOrden.Where(rep => rep.idOrdenTrabajo == id).ToList();
				if (repuestosOrden != null)
					return View("ListCost", repuestosOrden);
				else
					return HttpNotFound();
			}
			else
				return HttpNotFound();

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
