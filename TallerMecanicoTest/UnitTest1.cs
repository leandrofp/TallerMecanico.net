using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication4.Controllers;

namespace StoreTests.Controllers
{
	[TestClass]
	public class TallerMecanicoTest
	{
		[TestMethod]
		public void TestLoginRedirectEmpleadoPost()
		{



			var controller = new TallerMecanicoController();
			var result = (RedirectToRouteResult) controller.Login("pepe"); 
			Assert.AreEqual("indexEmpleado", result.RouteValues["action"]);
		}
		[TestMethod]
		public void TestLoginRedirectSupervisorPost()
		{

			var controller = new TallerMecanicoController();
			var result = (RedirectToRouteResult)controller.Login("lean");
			Assert.AreEqual("indexSupervisor", result.RouteValues["action"]);
		}
		[TestMethod]
		public void TestLoginErrorPost()
		{

			var controller = new TallerMecanicoController();
			var result = controller.Login("NULL") as ViewResult;
			Assert.AreEqual("Login", result.ViewName);
		}
	}
}
