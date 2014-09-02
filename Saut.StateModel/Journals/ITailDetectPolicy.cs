using System.Collections.Generic;

namespace Saut.StateModel.Journals
{
    /// <summary>�������� ������ ���������� ����������� �������� � ��������� ���������</summary>
    /// <typeparam name="TCollectionElementValue">��� �������� � ��������� ���������</typeparam>
    public interface ITailDetectPolicy<TCollectionElementValue>
    {
        /// <summary>���� ��������� ���������� ������� � ���������</summary>
        /// <param name="Records">��������� ��� ��������</param>
        /// <returns>�������, ����� �������� ����� ����������� �������� ���������</returns>
        ConcurrentLogNode<TCollectionElementValue> GetLastActualElement(IEnumerable<ConcurrentLogNode<TCollectionElementValue>> Records);
    }
}
