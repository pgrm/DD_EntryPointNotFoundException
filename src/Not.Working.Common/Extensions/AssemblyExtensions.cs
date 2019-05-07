using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Not.Working.Common.Extensions
{
    public static class AssemblyExtensions
    {
        private static IEnumerable<Assembly> _assembliesCollection;

        public static IEnumerable<Assembly> GetReferencedAssembliesRecursively(this Assembly assembly)
        {
            if (_assembliesCollection != null)
            {
                return _assembliesCollection;
            }

            _assembliesCollection = assembly.GetReferencedAssembliesRecursively(type => type.Namespace?.StartsWith("Not.", StringComparison.OrdinalIgnoreCase) ?? false);

            return _assembliesCollection;
        }

        private static IEnumerable<Assembly> GetReferencedAssembliesRecursively(this Assembly assembly, Func<Type, bool> typePredicateToExpandAssembly)
        {
            var assemblies = new List<Assembly>();
            var assembliesSet = new HashSet<string>();

            assemblies.Add(assembly);
            assembliesSet.Add(assembly.ToString());

            for (int i = 0; i < assemblies.Count; i++)
            {
                Assembly[] newAssemblies = assemblies[i].GetReferencedAssemblies()
                    .Where(a => !assembliesSet.Contains(a.ToString()))
                    .Select(Assembly.Load)
                    .Where(a => typePredicateToExpandAssembly == null || a.GetTypes().Any(typePredicateToExpandAssembly))
                    .ToArray();

                assemblies.AddRange(newAssemblies);

                foreach (Assembly newAssembly in newAssemblies)
                {
                    assembliesSet.Add(newAssembly.ToString());
                }
            }

            return assemblies;
        }
    }
}