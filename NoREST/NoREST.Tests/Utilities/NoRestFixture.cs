using AutoFixture;
using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NoREST.Tests.Utilities
{
    internal static class NoRestFixture
    {
        internal static IFixture Create()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new NoInversePropertySpecimenBuilder());
            return fixture;
        }
    }


    /// <summary>
    /// This is admittedly copy/paste from something I built elsewhere. Long story short,
    /// this tells the IFixture not to build anything marked with the InversePropertyAttribute.
    /// Otherwise, the creation algorithm goes into an infinite recursion because of the circular
    /// navigation between entities.
    /// </summary>
    public class NoInversePropertySpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var propInfo = request as MemberInfo;
            if (propInfo != null && propInfo.GetCustomAttribute(typeof(InversePropertyAttribute)) != null)
            {
                return new OmitSpecimen();
            }

            return new NoSpecimen();
        }
    }
}
