using System.Web;
using System.Web.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication4.Controllers;

using NUnit.Framework;
using Moq;
using WebApplication4.ModelAccess;
using WebApplication4.Models;
using System.Collections.Generic;

namespace StoreTests.Controllers
{
	[TestFixture]
	//[TestClass]
	public class TallerMecanicoTest
	{

		[TestCase("null", null)]	// algo que no exista en la BD
		[TestCase("pepe", "pepe")]
		[TestCase("lean", "lean")]
		public void TallerMecanicoEmpleadoFindTest(string username , string expectedName)
		{
			//Create Fake Object
			var fakeObject = new Mock<ITallerMecanicoAccess>(MockBehavior.Strict);
			//Set the Mock Configuration
			//The CheckDeptExist() method is call is set with the Integer parameter type
			//The Configuration also defines the Return type from the method  
			fakeObject.Setup(x => x.GetEmpleado(username)).Returns(true);
			//Call the methid
			var Res = fakeObject.Object.GetEmpleado(username);

			// esperado / actual / delta
			Assert.That( Res , Is.True);
		}



		[TestCase("null", "Login")]    // algo que no exista en la BD
		[TestCase("pepe", "indexEmpleado")]
		[TestCase("lean", "indexSupervisor")]
		public void TallerMecanicoLoginTest(string username, string expectedRoute)
		{
			var controller = new TallerMecanicoController();

			if (!username.Equals("null"))
			{
				var result = (RedirectToRouteResult)controller.Login(username);
				Assert.AreEqual(expectedRoute, result.RouteValues["action"]);
			}
			else
			{
				var result = controller.Login(username) as ViewResult;
				Assert.AreEqual(expectedRoute, result.ViewName);
			}
		}
	}
}
