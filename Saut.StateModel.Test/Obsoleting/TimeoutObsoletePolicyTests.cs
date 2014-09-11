using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Obsoleting;

namespace Saut.StateModel.Test.Obsoleting
{
    [TestFixture]
    public class TimeoutObsoletePolicyTests
    {
        [Test, Description("Проверяет правильность работы политики устаревания на основании таймаута")]
        public void Test()
        {
            DateTime t0 = DateTime.Now.AddHours(12);
            var policy = new TimeoutObsoletePolicy(TimeSpan.FromSeconds(10));

            var pick = MockRepository.GenerateMock<IJournalPick<int>>();
            pick.Stub(p => p.RecordsAfter).Return(new[]
                                                  {
                                                      new JournalRecord<int>(t0.AddSeconds(5), 1),
                                                      new JournalRecord<int>(t0.AddSeconds(10), 2),
                                                      new JournalRecord<int>(t0.AddSeconds(15), 3)
                                                  });
            pick.Stub(p => p.RecordsBefore).Return(new[]
                                                   {
                                                       new JournalRecord<int>(t0.AddSeconds(-5), 1),
                                                       new JournalRecord<int>(t0.AddSeconds(-10), 2),
                                                       new JournalRecord<int>(t0.AddSeconds(-15), 3)
                                                   });

            IJournalPick<int> decoratedPick = policy.DecoratePick(pick, t0);

            CollectionAssert.AreEquivalent(pick.RecordsAfter.Take(2), decoratedPick.RecordsAfter, "Не правильно рассчитана коллекция элементов \"ПОСЛЕ\"");
            CollectionAssert.AreEquivalent(pick.RecordsBefore.Take(2), decoratedPick.RecordsBefore, "Не правильно рассчитана коллекция элементов \"ПЕРЕД\"");
        }
    }
}
