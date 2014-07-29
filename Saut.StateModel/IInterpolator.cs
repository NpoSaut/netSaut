using System;

namespace Saut.StateModel
{
    public interface IInterpolator<TValue>
    {
        TValue Interpolate(IJournal<TValue> Journal, DateTime Time);
    }
}