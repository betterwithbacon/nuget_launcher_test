using NuGet;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using root_library;

namespace package_launcher_net
{
    public class NugetLoader
    {
        public IList<RootPackage> Packages { get; } = new List<RootPackage>();

        public void LoadNugetPackage(string packageName, string version = "1.0.0")
        {
            var repo = PackageRepositoryFactory.Default.CreateRepository("C:\\Development\\NugetPackages");

            string path = "c:\\temp";
            var packageManager = new PackageManager(repo, path);
            packageManager.PackageInstalled += PackageManager_PackageInstalled;

            var package = repo.FindPackage(packageName, SemanticVersion.Parse(version));
            if (package != null)
            {
                packageManager.InstallPackage(package, false, true);
            }
            else
            {
                Console.WriteLine("package couldn't be found.");
            }
        }

        private void PackageManager_PackageInstalled(object sender, PackageOperationEventArgs e)
        {
            var files = e.FileSystem.GetFiles(e.InstallPath, "*.dll", true);
            foreach (var file in files)
            {
                try
                {
                    AppDomain domain = AppDomain.CreateDomain("tmp");
                    Type typeProxyType = typeof(TypeProxy);

                    var assembly = domain.Load(typeProxyType.Assembly.FullName);
                    var allRootTypes = assembly.DefinedTypes.Where(typeinfo => typeinfo.IsAssignableFrom(typeof(root_library.RootPackage))).ToList();

                    //var typeProxyInstance = (TypeProxy)domain.CreateInstanceAndUnwrap(
                    //        typeProxyType.Assembly.FullName,
                    //        typeProxyType.FullName);

                    //var type = typeProxyInstance.LoadFromAssembly(file, "RootPackage, root_library");
                    //object instance =
                    //    domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
                    foreach(var type in allRootTypes)
                    {
                        var package = Activator.CreateInstance(type.AsType()) as RootPackage;
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

    public class TypeProxy : MarshalByRefObject
    {
        public Type LoadFromAssembly(string assemblyPath, string typeName)
        {
            try
            {
                var asm = Assembly.LoadFile(assemblyPath);
                return asm.GetType(typeName);
            }
            catch (Exception) { return null; }
        }
    }
}
