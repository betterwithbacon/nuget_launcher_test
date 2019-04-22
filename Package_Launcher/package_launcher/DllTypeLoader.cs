using root_library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace package_launcher
{
    internal class DllTypeLoader
    {
        public DllTypeLoader()
        {
        }

        public IList<RootPackage> Packages { get; } = new List<RootPackage>();

        public void LoadPackage(string folder)
        {
            var files = Directory.GetFiles(folder, "*.dll");            
            foreach (var file in files)
            {
                try
                {
                    var assembly = Assembly.LoadFile(file);
                    var allRootTypes = assembly.ExportedTypes.Where(typeinfo => typeof(RootPackage).IsAssignableFrom(typeinfo)).ToList();

                    foreach (var type in allRootTypes)
                    {
                        var package = Activator.CreateInstance(type) as RootPackage;
                        Packages.Add(package);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("failed to load {0}", file);
                    Console.WriteLine(ex.ToString());
                }
            }

        }
    }
}