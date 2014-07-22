using System.Collections.Generic;

namespace Modules.Dependencies
{
    public interface IDependenciesResolver
    {
        /// <summary>Определяет порядок инициализации модулей в соответствии с их зависимостями</summary>
        /// <param name="Modules">Список модулей для инициализации</param>
        /// <returns>Список модулей в порядке необходимости их инициализации</returns>
        /// <exception cref="UnresolvableDependenciesException">Не удалось разрешить дерево зависимостей</exception>
        IEnumerable<IModule> ResolveInitializationOrder(IEnumerable<IModule> Modules);
    }
}
