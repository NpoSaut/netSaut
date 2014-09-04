using System.Collections.Generic;

namespace Modules
{
    /// <summary>Инструмент, выполняющий запуск модулей</summary>
    public interface IModulesLauncher
    {
        /// <summary>Запускает все модули из списка</summary>
        /// <param name="Modules">Список модулей для запуска</param>
        void RunModules(IList<IExecutableModule> Modules);
    }
}
