using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TaskManagement.Repositories.TaskRepository;
using TaskManagement.Services.TaskService;
using Autofac;
using Autofac.Integration.WebApi;

namespace TaskManagement
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            ConfigureAutofac(GlobalConfiguration.Configuration);
        }

        private void ConfigureAutofac(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<TaskRepository>()
                   .As<ITaskRepository>()
                   .InstancePerRequest();

            builder.RegisterType<TaskService>()
                   .As<ITaskService>()
                   .InstancePerRequest();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
