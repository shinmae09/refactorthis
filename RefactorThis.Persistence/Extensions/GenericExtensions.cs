using RefactorThis.Persistence.Constants;
using System;

namespace RefactorThis.Persistence.Extensions
{
    public static class GenericExtensions
    {
        public static T ThrowIfNull<T>(this T obj, string paramName = null) where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName ?? nameof(obj), ExceptionMessage.OBJECT_CANNOT_BE_NULL_MESSAGE);
            }

            return obj;
        }
    }
}
