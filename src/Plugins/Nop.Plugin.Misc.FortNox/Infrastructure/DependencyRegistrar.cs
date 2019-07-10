using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Misc.FortNox.Data;
using Nop.Plugin.Misc.FortNox.Domain;
using Nop.Plugin.Misc.FortNox.Services;
using Nop.Services.Messages;
using Nop.Web.Framework.Infrastructure.Extensions;

namespace Nop.Plugin.Misc.FortNox.Infrastructure
{
    /// <summary>
    /// Represents a plugin dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            //register custom services
            builder.RegisterType<FortNoxService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<FortNoxManager>().AsSelf().InstancePerLifetimeScope();

            //override services

            //data context
            builder.RegisterPluginDataContext<FortNoxCustomerContext>("nop_object_context_misc_fort_nox_customer");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<FortNoxCustomerRecord>>().As<IRepository<FortNoxCustomerRecord>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_misc_fort_nox_customer"))
                .InstancePerLifetimeScope();
        }



        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order => 2;
    }
}