using System.Collections.Generic;
using Modules.Exceptions;
using NUnit.Framework;
using Rhino.Mocks;

namespace Modules.Test
{
    [TestFixture]
    public class BootstrapperTests
    {
        [Test, Description("Проверяет выбрасывание исключения при попытке выполнить модули до их инициализации")]
        [ExpectedException(typeof (ModulesWasNotInitializedException))]
        public void TestModulesNotInitializedFires()
        {
            var bs = MockRepository.GeneratePartialMock<BootstrapperBase>();
            bs.Run();
        }
    }
}