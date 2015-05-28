using Marketplace.Interview.Business.Core;
using Marketplace.Interview.Business.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Marketplace.Interview.Web.App_Start
{
    public static class UnitOfWorkPerRequestConfigure
    {
        private const string UOW_KEY = "UnitOfWork";

        public static void RegisterUnitOfWorkPerRequest(HttpApplication app)
        {
            app.BeginRequest += app_BeginRequest;
            app.EndRequest += app_EndRequest;
            app.Error += app_Error;
        }

        static void app_Error(object sender, EventArgs e)
        {
            var unitOfWork = GetUOWFromCurrentContext();
            unitOfWork.Rollback();
        }

        static void app_EndRequest(object sender, EventArgs e)
        {
            var unitOfWork = GetUOWFromCurrentContext();
            unitOfWork.Commit();
        }

        static void app_BeginRequest(object sender, EventArgs e)
        {
            var unitOfWork = ObjectFactory.GetObject<IUnitOfWork>(typeof(IUnitOfWork));
            HttpContext.Current.Items[UOW_KEY] = unitOfWork;
        }

        private static IUnitOfWork GetUOWFromCurrentContext()
        {
            var unitOfWork = HttpContext.Current.Items[UOW_KEY] as IUnitOfWork;
            if (unitOfWork == null)
                throw new NullReferenceException("no UnitOfWork from current context");
            return unitOfWork;
        }
    }
}
