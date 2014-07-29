using System;
using System.Collections.Generic;

namespace Saut.StateModel
{
    public interface IRecordPicker
    {
        IJournalPick<TValue> Pickrecords<TValue>(IJournal<TValue> Journal, DateTime Time);
    }
}
