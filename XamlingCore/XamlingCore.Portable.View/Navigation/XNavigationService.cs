using System;
using System.Collections.Generic;
using System.Linq;
using XamlingCore.Portable.Contract.Navigation;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Model.Navigation;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Portable.View.ViewModel.Base;

namespace XamlingCore.Portable.View.Navigation
{
    public class XNavigationService : NotifyBase, IXNavigation
    {
        private bool _isReverseNavigation;

        private List<object> _navigationHistory;

        private object _currentContentObject;
        private object _previousObject;

        private object _modalContentObject;

        private bool _canGoBack;

        public bool IsModal { get; set; }

        public event EventHandler<XNavigationEventArgs> Navigated;

        public XNavigationService()
        {
            NavigationHistory = new List<object>();
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

        public void NavigateToModal(object content)
        {
            var dispose = ModalContentObject as IDisposable;

            if (dispose != null)
            {
                dispose.Dispose();
            }

            ModalContentObject = content;

            if (ModalContentObject == null)
            {
                _disposeHistory();
            }

            if (Navigated != null)
            {
                Navigated(this, new XNavigationEventArgs(NavigationDirection.Modal));
            }
        }

        public void NavigateTo(object content)
        {
            NavigateTo(content, false);
        }

        public void NavigateTo(object content, bool noHistory)
        {
            NavigateTo(content, noHistory, false);
        }

        public void NavigateTo(object content, bool noHistory, bool forceBack)
        {
            if (content == null && IsModal)
            {
                NavigateToModal(null);
                return;
            }

            if (content != null)
            {

                if (CurrentContentObject != null)
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

            new NavigationMessage(CurrentContentObject, IsReverseNavigation).Send();

            if (Navigated != null)
            {
                Navigated(this, new XNavigationEventArgs(IsReverseNavigation ? NavigationDirection.Back : NavigationDirection.Forward));
            }
        }

        public bool NavigateBack()
        {
            return NavigateBack(false);
        }

        public bool NavigateBack(bool allowNullNavigation)
        {
            if (IsModal && _navigationHistory.Count == 0)
            {
                NavigateToModal(null);
                return true;
            }

            if (_navigationHistory.Count > 1)
            {
                NavigateTo(_navigationHistory[_navigationHistory.Count - 1], false, true);
                return true;
            }
            else if (_navigationHistory.Count == 1)
            {
                NavigateTo(_navigationHistory[0], true, true);
                return true;
            }
            else if (allowNullNavigation)
            {
                NavigateTo(null, false, true);
                return true;
            }

            return false;
        }

        void _disposeHistory()
        {
            do
            {
                var item = _navigationHistory.LastOrDefault();
                
                var itemDisposable = item as IDisposable;

                if (itemDisposable != null)
                {
                    itemDisposable.Dispose();
                }

                _navigationHistory.Remove(item);

            } while (_navigationHistory.Count > 0);

            var ccoDisposable = CurrentContentObject as IDisposable;
            
            if (ccoDisposable != null)
            {
                ccoDisposable.Dispose();
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

        public object ModalContentObject
        {
            get { return _modalContentObject; }
            set
            {
                _modalContentObject = value;
                OnPropertyChanged("ModalContentObject");
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
}
