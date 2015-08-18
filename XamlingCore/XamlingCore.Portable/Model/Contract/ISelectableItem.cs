using System;

namespace XamlingCore.Portable.Model.Contract
{
    public interface ISelectableItem<T>
    {
        T Item { get; }
        event EventHandler SelectionChanged;
    }
}
