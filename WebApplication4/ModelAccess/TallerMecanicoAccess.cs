using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WebApplication4.Models;

namespace WebApplication4.ModelAccess
{
	/// <summary>
	/// Interface for providing functionality for Department Data Access
	/// </summary>
	public interface ITallerMecanicoAccess
	{
		bool GetEmpleado(string username);



		//Department GetDepartment(int id);
		//void CreateDepartment(Department dept);
		//bool CheckDeptExist(int id);
	}

	/// <summary>
	/// The Department Data Access
	/// </summary>
	public class TallerMecanicoAccess : ITallerMecanicoAccess
	{
		TallerMecanicoEntities tme;
		public TallerMecanicoAccess()
		{
			tme = new TallerMecanicoEntities();
		}
		/// <summary>
		/// Get All Departments
		/// </summary>
		/// <returns></returns>
		public bool GetEmpleado(string username)
		{
			var emps = tme.Empleado.ToList();
			foreach (Empleado emp in emps)
			{
				if (emp.nombre == username)
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Get Department base on Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/*public Department GetDepartment(int id)
		{
			var dept = ctx.Departments.Find(id);
			return dept;
		}
		/// <summary>
		/// Create Department
		/// </summary>
		/// <param name="dept"></param>
		public void CreateDepartment(Department dept)
		{
			ctx.Departments.Add(dept);
			ctx.SaveChanges();
		}
		/// <summary>
		/// Check whether Department Exist or Not
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool CheckDeptExist(int id)
		{
			var dept = ctx.Departments.Find(id);
			if (dept != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}*/
	}


	/*public class TallerMecanicoAccess
	{
	}*/

}