using BoilerMVC.Common;
using BoilerMVC.Data;
using Ninject.Modules;
using Ninject.Web.Common;

namespace BoilerMVC.Framework
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(typeof (IUnitOfWork)).To<BoilerMVCEntities>().InRequestScope();
            Kernel.Bind(typeof(IRepository<>)).To(typeof(EntityRepository<>));
        }
    }
}