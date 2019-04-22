using NuGet;
using System;
using System.Threading.Tasks;

namespace package_launcher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await Test1_NugetPackageEmbedded();
            await Test2_NugetPackageLoaded();
        }

        //public static async Task Test1_NugetPackageEmbedded()
        //{
        //    var nuget = new LocalNugetTestPack.Class1();

        //    await nuget.Do((val) => Console.WriteLine(val));
        //}

        public static async Task Test2_NugetPackageLoaded()
        {
            var loader = new NugetLoader();
            loader.LoadNugetPackage("LocalNugetTestPack");


            foreach(var package in loader.Packages)
            {
                await package.Do((val) => Console.WriteLine(val));
            }

            await Task.CompletedTask;
        }
    }    
}
