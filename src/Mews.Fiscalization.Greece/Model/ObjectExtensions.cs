using FuncSharp;
using Mews.Fiscalization.Core.Model;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model
{
    public static class ObjectExtensions
    {
        public static ITry<T, IEnumerable<Error>> NotNull<T>(T value)
        {
            return value.ToTry(v => v.IsNotNull(), _ => new Error("Value cannot be null.").ToEnumerable());
        }
    }
}
