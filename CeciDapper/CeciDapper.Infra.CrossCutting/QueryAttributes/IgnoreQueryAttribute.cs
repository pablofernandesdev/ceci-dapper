using System;

namespace CeciDapper.Infra.CrossCutting.QueryAttributes
{
    /// <summary>
    /// Specifies that a property should be ignored in query generation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreQueryAttribute : Attribute
    {
    }
}
