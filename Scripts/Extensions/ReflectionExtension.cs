namespace GDK.Scripts.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ReflectionExtension
    {
        public static bool IsSubclassOfRawGeneric(this Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }

        public static IEnumerable<Type> GetAllNonAbstractDerivedTypeFrom<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(domainAssembly => domainAssembly.GetTypes())
                .Where(type => !type.IsAbstract && typeof(T).IsAssignableFrom(type));
        }
    }
}