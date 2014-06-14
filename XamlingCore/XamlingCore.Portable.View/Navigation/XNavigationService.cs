using System;
using System.Collections.Generic;
using XamlingCore.Portable.Contract.Navigation;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Portable.View.ViewModel.Base;

namespace XamlingCore.Portable.View.Navigation
{
    public class XNavigationService : NotifyBase, IXNavigationService
    {
        private bool _isReverseNavigation;

        private List<object> _navigationHistory;

        private object _currentContentObject;
        private object _previousObject;

        private bool _canGoBack;

        public event EventHandler Navigated;

        public XRootViewModelBase ParentRoot { get; set; }

        public DateTime? LastNavigation { get; private set; }

        public XNavigationService(XRootViewModelBase parentRoot)
        {
            NavigationHistory = new List<object>();
            ParentRoot = parentRoot;
        }

        public void InsertIntoHistory(object obj)
        {
            _navigationHistory.Add(obj);
        }

        public void ResetHistory()
        {
            _navigationHistory.Clear();
            CanGoBack = false;
        }

        public void NavigateTo(object content)
        {
            NavigateTo(content, false);
        }

        public void NavigateTo(object content, bool noHistory)
        {
            NavigateTo(content, noHistory, false);
        }

        public bool IsInNavigationCooldown()
        {
            return (LastNavigation.HasValue && DateTime.Now.Subtract(LastNavigation.Value) < TimeSpan.FromSeconds(2));
        }

        public void NavigateTo(object content, bool noHistory, bool forceBack)
        {
            if (IsInNavigationCooldown())
            {
                return;
            }

            LastNavigation = DateTime.Now;

            ParentRoot.OnNavigating(content, CurrentContentObject);
            if (content != null)
            {

                if (_navigationHistory.Count > 0 && CurrentContentObject != null)
                {
                    PreviousContentObject = CurrentContentObject;
                }
                else
                {
                    PreviousContentObject = null;
                }

                if (_navigationHistory.Contains(content))
                {
                    IsReverseNavigation = true;
                    while (_navigationHistory[_navigationHistory.Count - 1] != content)
                    {
                        var item = _navigationHistory[_navigationHistory.Count - 1];
                        var dispose = item as IDisposable;

                        if (dispose != null)
                        {
                            dispose.Dispose();
                        }

                        _navigationHistory.Remove(item);
                    }

                    _navigationHistory.Remove(content);
                }
                else
                {
                    IsReverseNavigation = false;

                    if (!noHistory && CurrentContentObject != null)
                    {
                        _navigationHistory.Add(CurrentContentObject);
                    }

                }

                CanGoBack = NavigationHistory.Count > 0;





                if (forceBack)
                {
                    IsReverseNavigation = true;
                }

                CurrentContentObject = content;

                if (PreviousContentObject != null && PreviousContentObject != content)
                {
                    var contentModel = PreviousContentObject as XViewModel;
                    if (contentModel != null)
                    {
                        contentModel.OnDeactivated();
                    }

                    if (IsReverseNavigation || noHistory) //this item cannot be used again
                    {
                        var disp = PreviousContentObject as IDisposable;

                        if (disp != null)
                        {
                            disp.Dispose();
                        }
                    }
                }

                var currentContentAsModel = CurrentContentObject as XViewModel;

                if (currentContentAsModel != null)
                {
                    currentContentAsModel.OnActivated();
                }
            }
            else
            {
                IsReverseNavigation = true;
                CurrentContentObject = null;
            }

            if (Navigated != null)
            {
                Navigated(this, EventArgs.Empty);
            }
        }

        public void NavigateBack()
        {
            NavigateBack(false);
        }

        public void NavigateBack(bool allowNullNavigation)
        {
            if (_navigationHistory.Count > 1)
            {
                NavigateTo(_navigationHistory[_navigationHistory.Count - 1], false, true);
            }
            else if (_navigationHistory.Count == 1)
            {
                NavigateTo(_navigationHistory[0], true, true);
            }
            else if (allowNullNavigation)
            {
                NavigateTo(null, false, true);
            }
        }

        public bool IsReverseNavigation
        {
            get { return _isReverseNavigation; }
            set
            {
                _isReverseNavigation = value;
                OnPropertyChanged("IsReverseNavigation");
            }
        }


        public List<object> NavigationHistory
        {
            get { return _navigationHistory; }
            set
            {
                _navigationHistory = value;
                OnPropertyChanged("NavigationHistory");
            }
        }

        public object CurrentContentObject
        {
            get { return _currentContentObject; }
            set
            {
                _currentContentObject = value;

                OnPropertyChanged("CurrentContentObject");
            }
        }

        public object PreviousContentObject
        {
            get { return _previousObject; }
            set
            {
                _previousObject = value;
                OnPropertyChanged("PreviousContentObject");
            }
        }

        public bool CanGoBack
        {
            get { return _canGoBack; }
            set
            {
                _canGoBack = value;
                OnPropertyChanged("CanGoBack");
            }
        }
    }


    public enum NavigationDirection
    {
        Forward,
        Back
    }
}
