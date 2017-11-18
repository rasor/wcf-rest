using Microsoft.Practices.Unity;
using System.Diagnostics;
using Unity.Wcf;
using WebApplicationWcfRest1.interfaces;

namespace WebApplicationWcfRest1
{
	public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            Debug.WriteLine("");
            // register all your components with the container here
            container
               .RegisterType<IBookService, RestService1>();
               //.RegisterType<DataContext>(new HierarchicalLifetimeManager());
        }
    }    
}