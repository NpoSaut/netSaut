namespace Saut.StateModel.Interfaces
{
    /// <summary>Фабрика журналов для свойств</summary>
    /// <typeparam name="TJournalValue">Тип значений в журнале</typeparam>
    public interface IJournalFactory<TJournalValue>
    {
        IJournal<TJournalValue> GetJournal();
    }
}
