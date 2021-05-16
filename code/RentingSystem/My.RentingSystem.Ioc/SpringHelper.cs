using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spring.Context.Support;

namespace My.RentingSystem.Ioc
{
    public class SpringHelper
    {
        private static IApplicationContext SpringContext 
        {
            get 
            {
                return ContextRegistry.GetContext();
            }
        }

        public static T GetObject<T>(string objName) where T : class
        {
            return (T)SpringContext.GetObject(objName);
        }

    }
}
