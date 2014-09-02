using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Journals;

namespace Saut.StateModel.Test.Journals
{
    [TestFixture]
    public class TailCutLinkedNodesCollectionCleanerTests
    {
        [Test, Description("Проверяет, обрезает ли он всё после указанной даты")]
        public void TestCut()
        {
            var nodeX = new ConcurrentLogNode<int>(100);
            var collection = Enumerable.Range(0, 20).Select(i => new ConcurrentLogNode<int>(i) { Next = nodeX}).ToList();

            var policy = MockRepository.GenerateMock<ITailDetectPolicy<int>>();
            policy.Expect(p => p.GetLastActualElement(collection)).Return(collection[2]);
            
            var cutter = new TailCutLinkedNodesCollectionCleaner<int>(policy);
            cutter.Cleanup(collection);

            policy.VerifyAllExpectations();
            Assert.AreEqual(collection[2].Next, null);
        }
    }
}
