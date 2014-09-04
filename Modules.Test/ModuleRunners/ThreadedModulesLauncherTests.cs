using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Modules.ModuleRunners;
using NUnit.Framework;
using Rhino.Mocks;

namespace Modules.Test.ModuleRunners
{
    [TestFixture]
    public class ThreadedModulesLauncherTests
    {
        [Test, Description("Проверяет то, что каждый модуль запускается в отдельном потоке, один из них -- в изначальном потоке")]
        public void TestRunEveryoneInSeparateThread()
        {
            const int modulesCount = 6;

            var threadIdsBag = new ConcurrentBag<int>();
            List<IExecutableModule> modules =
                Enumerable.Range(0, modulesCount)
                          .Select(i =>
                                  {
                                      var moduleMock = MockRepository.GenerateMock<IExecutableModule>();
                                      moduleMock.Expect(m => m.Run())
                                                .WhenCalled(Invocation => threadIdsBag.Add(Thread.CurrentThread.ManagedThreadId))
                                                .Message("Для каждого модуля должен быть вызван метод Run()");
                                      return moduleMock;
                                  })
                          .ToList();

            int myThreadId = Thread.CurrentThread.ManagedThreadId;
            var launcher = new ThreadedModulesLauncher();
            launcher.RunModules(modules);

            SpinWait.SpinUntil(() => threadIdsBag.Count == modulesCount);

            foreach (IExecutableModule module in modules)
                module.VerifyAllExpectations();

            var threadIds = threadIdsBag.ToList();
            CollectionAssert.AllItemsAreUnique(threadIdsBag, "Какой-то из модулей не был запущен в отдельном потоке");
            CollectionAssert.Contains(threadIds, myThreadId, "Ни один из модулей не был запущен в вызывающем потоке");
        }
    }
}
