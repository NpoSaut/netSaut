using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Test
{
    [TestFixture]
    public class RecordPickerTest
    {
        [Test]
        public void PickUpTest()
        {
            DateTime t0 = DateTime.Today;
            var records = new[]
                          {
                              new JournalRecord<int>(t0.AddMilliseconds(10000), 350), // 0
                              new JournalRecord<int>(t0.AddMilliseconds(8000), 340), // 1
                              new JournalRecord<int>(t0.AddMilliseconds(6000), 330), // 2
                              // Test time is here
                              new JournalRecord<int>(t0.AddMilliseconds(4000), 320), // 3
                              new JournalRecord<int>(t0.AddMilliseconds(2000), 310), // 4
                              new JournalRecord<int>(t0.AddMilliseconds(0000), 300) // 5
                          };
            var journalMock = MockRepository.GenerateMock<IJournal<int>>();
            journalMock
                .Expect(j => j.Records)
                .Return(
                    records);
            var picker = new RecordPicker();
            IJournalPick<int> pick = picker.PickRecords(journalMock, t0.AddMilliseconds(5000));

            List<JournalRecord<int>> listBefore = pick.RecordsBefore.ToList();
            List<JournalRecord<int>> listAfter = pick.RecordsAfter.ToList();

            journalMock.VerifyAllExpectations();
            Assert.AreEqual(records[3], listBefore[0], "Не правильно найдено событие №1 из очереди \"До указанного времени\"");
            Assert.AreEqual(records[4], listBefore[1], "Не правильно найдено событие №2 из очереди \"До указанного времени\"");
            Assert.AreEqual(records[5], listBefore[2], "Не правильно найдено событие №3 из очереди \"До указанного времени\"");
            Assert.AreEqual(records[2], listAfter[0], "Не правильно найдено событие №1 из очереди \"После указанного времени\"");
            Assert.AreEqual(records[1], listAfter[1], "Не правильно найдено событие №2 из очереди \"После указанного времени\"");
            Assert.AreEqual(records[0], listAfter[2], "Не правильно найдено событие №3 из очереди \"После указанного времени\"");
        }
    }
}
