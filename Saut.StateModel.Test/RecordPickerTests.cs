using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Test
{
    [TestFixture]
    public class RecordPickerTests
    {
        private DateTime _t0;
        private readonly IJournal<int> _testJournal;
        private readonly JournalRecord<int>[] _testRecords;

        public RecordPickerTests()
        {
            _t0 = DateTime.Today.AddHours(12);
            _testRecords = new[]
                          {
                              new JournalRecord<int>(_t0.AddMilliseconds(10000), 350), // 0
                              new JournalRecord<int>(_t0.AddMilliseconds(8000), 340), // 1
                              new JournalRecord<int>(_t0.AddMilliseconds(6000), 330), // 2
                              // Middle test time is here
                              new JournalRecord<int>(_t0.AddMilliseconds(4000), 320), // 3
                              new JournalRecord<int>(_t0.AddMilliseconds(2000), 310), // 4
                              new JournalRecord<int>(_t0.AddMilliseconds(0000), 300) // 5
                          };
            _testJournal = MockRepository.GenerateMock<IJournal<int>>();
            _testJournal
                .Stub(j => j.Records)
                .Return(
                    _testRecords);
        }

        [Test, Description("Проверка выборки в середине участка")]
        public void MiddlePickUpTest()
        {
            var picker = new RecordPicker();
            IJournalPick<int> pick = picker.PickRecords(_testJournal, _t0.AddMilliseconds(5000));

            List<JournalRecord<int>> listBefore = pick.RecordsBefore.ToList();
            List<JournalRecord<int>> listAfter = pick.RecordsAfter.ToList();

            Assert.AreEqual(_testRecords[3], listBefore[0], "Не правильно найдено событие №1 из очереди \"До указанного времени\"");
            Assert.AreEqual(_testRecords[4], listBefore[1], "Не правильно найдено событие №2 из очереди \"До указанного времени\"");
            Assert.AreEqual(_testRecords[5], listBefore[2], "Не правильно найдено событие №3 из очереди \"До указанного времени\"");
            Assert.AreEqual(_testRecords[2], listAfter[0], "Не правильно найдено событие №1 из очереди \"После указанного времени\"");
            Assert.AreEqual(_testRecords[1], listAfter[1], "Не правильно найдено событие №2 из очереди \"После указанного времени\"");
            Assert.AreEqual(_testRecords[0], listAfter[2], "Не правильно найдено событие №3 из очереди \"После указанного времени\"");
        }

        [Test, Description("Проверка выборки в середине участка")]
        public void OnPointPickUpTest()
        {
            var picker = new RecordPicker();
            IJournalPick<int> pick = picker.PickRecords(_testJournal, _t0.AddMilliseconds(6000));

            List<JournalRecord<int>> listBefore = pick.RecordsBefore.ToList();
            List<JournalRecord<int>> listAfter = pick.RecordsAfter.ToList();

            Assert.AreEqual(_testRecords[2], listBefore[0], "Не правильно найдено событие №1 из очереди \"До указанного времени\", которое совпадает с моментом выборки");
            Assert.AreEqual(_testRecords[3], listBefore[1], "Не правильно найдено событие №2 из очереди \"До указанного времени\"");
            Assert.AreEqual(_testRecords[4], listBefore[2], "Не правильно найдено событие №3 из очереди \"До указанного времени\"");
            Assert.AreEqual(_testRecords[5], listBefore[3], "Не правильно найдено событие №4 из очереди \"До указанного времени\"");
            Assert.AreEqual(_testRecords[1], listAfter[0], "Не правильно найдено событие №2 из очереди \"После указанного времени\"");
            Assert.AreEqual(_testRecords[0], listAfter[1], "Не правильно найдено событие №3 из очереди \"После указанного времени\"");
        }
    }
}
