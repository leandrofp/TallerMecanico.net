using System.Web;
using System.Web.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication4.Controllers;
using NUnit.Framework;

namespace StoreTests.Controllers
{
	[TestFixture]
	//[TestClass]
	public class TallerMecanicoTest
	{

		[TestCase("null", "Login")]	// algo que no exista en la BD
		[TestCase("pepe", "indexEmpleado")]
		[TestCase("lean", "indexSupervisor")]
		public void TallerMecanicoLoginTest(string username , string expectedRoute)
		{

			var controller = new TallerMecanicoController();

			if (!username.Equals("null")){
				var result = (RedirectToRouteResult)controller.Login(username);
				Assert.AreEqual(expectedRoute, result.RouteValues["action"]);
			}
			else
			{
				var result = controller.Login(username) as ViewResult;
				Assert.AreEqual(expectedRoute, result.ViewName);
			}



		}

		
		//CAMBIAR LIBRERIAS 
		/*
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
		}*/
	}
}
