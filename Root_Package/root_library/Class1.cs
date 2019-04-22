using System;
using System.Threading.Tasks;

namespace root_library
{
    public interface RootPackage
    {
        string Name { get; }
        Task Do(Action<string> logger);
    }
}
