using System;
using System.Collections.Generic;
using System.Linq;

namespace Modules.Dependencies
{
    /// <summary>Класс, занимающийся разрешением последовательности инициализации модулей</summary>
    public class DependenciesResolver : IDependenciesResolver
    {
        private readonly ModuleInformationFactory _moduleInformationFactory;

        public DependenciesResolver(ModuleInformationFactory ModuleInformationFactory) { _moduleInformationFactory = ModuleInformationFactory; }

        /// <summary>Определяет порядок инициализации модулей в соответствии с их зависимостями</summary>
        /// <param name="Modules">Список модулей для инициализации</param>
        /// <returns>Список модулей в порядке необходимости их инициализации</returns>
        /// <exception cref="UnresolvableDependenciesException">Не удалось разрешить дерево зависимостей</exception>
        public IEnumerable<IModule> ResolveInitializationOrder(IEnumerable<IModule> Modules)
        {
            var initializedTypes = new List<Type>();
            List<ModuleInformation> uninitializedModules = Modules.Select(_moduleInformationFactory.GetModuleInformation).ToList();
            while (uninitializedModules.Any())
            {
                ModuleInformation moduleForInitialization =
                    uninitializedModules.FirstOrDefault(
                        m =>
                        m.Dependencies.All(d => initializedTypes.Contains(d) &&
                                                !uninitializedModules.Any(mm => mm.Provides.Intersect(m.Dependencies).Any())));

                if (moduleForInitialization == null)
                    throw new UnresolvableDependenciesException(uninitializedModules.Select(mi => mi.Module).ToList(), initializedTypes);

                initializedTypes.AddRange(moduleForInitialization.Provides);
                uninitializedModules.Remove(moduleForInitialization);
                yield return moduleForInitialization.Module;
            }
        }
    }
}
