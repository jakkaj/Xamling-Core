using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Portable.View.ViewModel.Base;

namespace XamlingCore.Portable.View.Navigation
{
    public class XNavigationService : NotifyBase
    {
        private bool isReverseNavigation;

        private List<object> navigationHistory;

        private object currentContentObject;
        private object previousObject;

        private bool canGoBack;

        public event EventHandler Navigated;

        private object serialiseContentOject;

        public XFrameViewModelBase ParentFrame { get; set; }

        public DateTime? LastNavigation { get; private set; }

        public XNavigationService(XFrameViewModelBase parentFrame)
        {
            NavigationHistory = new List<object>();
            ParentFrame = parentFrame;
        }

        public void InsertIntoHistory(object obj)
        {
            navigationHistory.Add(obj);
        }

        public void Serialise(IEnumerable<Type> knownTypes)
        {


        }

        public void ResetHistory()
        {
            navigationHistory.Clear();
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

            ParentFrame.OnNavigating(content, CurrentContentObject);
            if (content != null)
            {

                if (navigationHistory.Count > 0 && CurrentContentObject != null)
                {
                    PreviousContentObject = CurrentContentObject;
                }
                else
                {
                    PreviousContentObject = null;
                }

                if (navigationHistory.Contains(content))
                {
                    IsReverseNavigation = true;
                    while (navigationHistory[navigationHistory.Count - 1] != content)
                    {
                        var item = navigationHistory[navigationHistory.Count - 1];
                        var dispose = item as IDisposable;

                        if (dispose != null)
                        {
                            dispose.Dispose();
                        }

                        navigationHistory.Remove(item);
                    }

                    navigationHistory.Remove(content);
                }
                else
                {
                    IsReverseNavigation = false;

                    if (!noHistory && CurrentContentObject != null)
                    {
                        navigationHistory.Add(CurrentContentObject);
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
            if (navigationHistory.Count > 1)
            {
                NavigateTo(navigationHistory[navigationHistory.Count - 1], false, true);
            }
            else if (navigationHistory.Count == 1)
            {
                NavigateTo(navigationHistory[0], true, true);
            }
            else if (allowNullNavigation)
            {
                NavigateTo(null, false, true);
            }
        }

        public bool IsReverseNavigation
        {
            get { return isReverseNavigation; }
            set
            {
                isReverseNavigation = value;
                OnPropertyChanged("IsReverseNavigation");
            }
        }


        public List<object> NavigationHistory
        {
            get { return navigationHistory; }
            set
            {
                navigationHistory = value;
                OnPropertyChanged("NavigationHistory");
            }
        }

        public object CurrentContentObject
        {
            get { return currentContentObject; }
            set
            {
                currentContentObject = value;

                if (CurrentContentObject != null)
                {
                    SerialiseContentObject = value;
                }

                OnPropertyChanged("CurrentContentObject");
            }
        }


        public object SerialiseContentObject
        {
            get { return serialiseContentOject; }
            set { serialiseContentOject = value; }
        }


        public object PreviousContentObject
        {
            get { return previousObject; }
            set
            {
                previousObject = value;
                OnPropertyChanged("PreviousContentObject");
            }
        }


        public bool CanGoBack
        {
            get { return canGoBack; }
            set
            {
                canGoBack = value;
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
