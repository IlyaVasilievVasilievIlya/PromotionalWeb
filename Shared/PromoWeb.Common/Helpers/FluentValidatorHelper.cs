using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoWeb.Common.Helpers
{
    public static class FluentValidatorHelper
    {
        public static void Register(IServiceCollection services)
        {
            var validators = from type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                             where !type.IsAbstract && !type.IsGenericTypeDefinition
                             let interfaces = type.GetInterfaces()
                             let genericInterfaces = interfaces.Where(i =>
                                 i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>))
                             let matchingInterface = genericInterfaces.FirstOrDefault()
                             where matchingInterface != null
                             select new
                             {
                                 InterfaceType = matchingInterface,
                                 ValidatorType = type
                             };

            validators.ToList().ForEach(x => { services.AddSingleton(x.InterfaceType, x.ValidatorType); });
        }
    }
}
