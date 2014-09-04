namespace Modules
{
    /// <summary>Модуль, который может выполнять работу</summary>
    public interface IExecutableModule : IModule
    {
        /// <summary>Основная рабочая процедура модуля</summary>
        void Run();
    }
}
