namespace Saut.StateModel.Obsoleting
{
    /// <summary>Генератор политик устаревания для свойства</summary>
    public interface IObsoletePolicyProvider
    {
        /// <summary>Получает политику устаревания свойства</summary>
        /// <param name="Property">Свойство, для которого требуется получить политику устаревания</param>
        /// <returns>Политика устаревания для указанного свойства</returns>
        IObsoletePolicy GetObsoletePolicy(IStateProperty Property);
    }
}
