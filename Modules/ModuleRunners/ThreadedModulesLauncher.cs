using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Modules.ModuleRunners
{
    /// <summary>Потоковый лаунчер, запускающий каждый модуль в отдельном потоке, первый модуль - в своём</summary>
    public class ThreadedModulesLauncher : IModulesLauncher
    {
        /// <summary>Запускает все модули из списка</summary>
        /// <param name="Modules">Список модулей для запуска</param>
        public void RunModules(IList<IExecutableModule> Modules)
        {
            if (!Modules.Any()) return;
            foreach (IExecutableModule module in Modules.Skip(1))
            {
                var moduleThread = new Thread(module.Run);
                moduleThread.Start();
            }
            Modules.First().Run();
        }
    }
}
