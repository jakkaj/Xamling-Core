using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace XamlingCore.Portable.Contract.Navigation
{
    public interface IXNavigationService
    {
        event EventHandler Navigated;
        DateTime? LastNavigation { get; }
        bool IsReverseNavigation { get; set; }
        List<object> NavigationHistory { get; set; }
        object CurrentContentObject { get; set; }
        object PreviousContentObject { get; set; }
        bool CanGoBack { get; set; }
        void InsertIntoHistory(object obj);
        void ResetHistory();
        void NavigateTo(object content);
        void NavigateTo(object content, bool noHistory);
        bool IsInNavigationCooldown();
        void NavigateTo(object content, bool noHistory, bool forceBack);
        void NavigateBack();
        void NavigateBack(bool allowNullNavigation);
        event PropertyChangedEventHandler PropertyChanged;
    }
}