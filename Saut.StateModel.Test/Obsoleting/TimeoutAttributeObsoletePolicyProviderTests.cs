using System;
using NUnit.Framework;
using Saut.StateModel.Obsoleting;

namespace Saut.StateModel.Test.Obsoleting
{
    [TestFixture]
    public class TimeoutAttributeObsoletePolicyProviderTests
    {
        private const int DefaultTimout = 7000;
        private const int BaseTimout = 1000;
        private const int OverriddenTimout = 5500;

        private class PropertyWithoutAttribute : IStateProperty
        {
            public string Name { get; private set; }
            public bool HaveValue() { throw new NotImplementedException(); }
        }

        [ObsoleteTimeout(BaseTimout)]
        private abstract class BasePropertyWithAttribute : IStateProperty
        {
            public string Name { get; private set; }
            public bool HaveValue() { throw new NotImplementedException(); }
        }

        private class OverriddenPropertyWithoutAttribute : BasePropertyWithAttribute { }

        [ObsoleteTimeout(OverriddenTimout)]
        private class OverriddenPropertyWithAttribute : BasePropertyWithAttribute { }

        [Test, Description("Проверяет правильность работы выбора времени устаревания из атрибута базового свойства")]
        public void BaseClassAttributeTimeoutTest()
        {
            var provider = new TimeoutAttributeObsoletePolicyProvider();
            IObsoletePolicy policy = provider.GetObsoletePolicy(new OverriddenPropertyWithoutAttribute());

            Assert.IsInstanceOf<TimeoutObsoletePolicy>(policy, "Была создана политика неверного типа");
            var timeoutPolicy = (TimeoutObsoletePolicy)policy;
            Assert.AreEqual(TimeSpan.FromMilliseconds(BaseTimout), timeoutPolicy.ObsoleteTimeout, "Было выбрано неверное время устаревания");
        }

        [Test, Description("Проверяет правильность работы выбора времени устаревания по-умолчанию для свойств с не указанным атрибутом")]
        public void DefaultTimeoutTest()
        {
            var provider = new TimeoutAttributeObsoletePolicyProvider { DefaultObsoleteTimeout = TimeSpan.FromMilliseconds(DefaultTimout) };
            IObsoletePolicy policy = provider.GetObsoletePolicy(new PropertyWithoutAttribute());

            Assert.IsInstanceOf<TimeoutObsoletePolicy>(policy, "Была создана политика неверного типа");
            var timeoutPolicy = (TimeoutObsoletePolicy)policy;
            Assert.AreEqual(TimeSpan.FromMilliseconds(DefaultTimout), timeoutPolicy.ObsoleteTimeout, "Было выбрано неверное время устаревания");
        }

        [Test, Description("Проверяет правильность работы выбора времени устаревания из переопределённого атрибута свойства")]
        public void OverriddenAttributeTimeoutTest()
        {
            var provider = new TimeoutAttributeObsoletePolicyProvider();
            IObsoletePolicy policy = provider.GetObsoletePolicy(new OverriddenPropertyWithoutAttribute());

            Assert.IsInstanceOf<TimeoutObsoletePolicy>(policy, "Была создана политика неверного типа");
            var timeoutPolicy = (TimeoutObsoletePolicy)policy;
            Assert.AreEqual(TimeSpan.FromMilliseconds(BaseTimout), timeoutPolicy.ObsoleteTimeout, "Было выбрано неверное время устаревания");
        }
    }
}
