using System;

namespace Saut.Communication.Interfaces
{
    /// <summary>������ ����� � ��������� �������� ���������</summary>
    public interface IMessageProcessingService : IDisposable
    {
        /// <summary>��������� ������ �������</summary>
        void Run();
    }
}
