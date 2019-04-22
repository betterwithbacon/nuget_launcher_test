using root_library;
using System;
using System.Threading.Tasks;

namespace LocalNugetTestPack
{
    public class Class1 : RootPackage
    {
        public string Name => "test app";

        public async Task Do(Action<string> logger)
        {
            await Task.Run(() => logger("hit"));
        }
    }
}
