//Thanks to https://raw.githubusercontent.com/XLabs/Xamarin-Forms-Labs/master/src/Forms/XLabs.Forms/Controls/RepeaterView.cs for the inspiration

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.XamarinThings.Content.Lists
{
    
    public class RepeaterView<T> : StackLayout
        where T : XViewModel
    {

        private IViewResolver _resolver;

        private ObservableCollection<T> _collection; 

       
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<RepeaterView<T>, ObservableCollection<T>>(
                p => p.ItemsSource,
                null,
                BindingMode.OneWay,
                null,
                ItemsChanged);

        
        public static BindableProperty ItemClickCommandProperty =
            BindableProperty.Create<RepeaterView<T>, ICommand>(x => x.ItemClickCommand, null);


        public RepeaterView()
        {
            Spacing = 0;
            _resolver = ContainerHost.Container.Resolve<IViewResolver>();
        }

     
        public ObservableCollection<T> ItemsSource
        {
            get { return (ObservableCollection<T>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
       

       
        public ICommand ItemClickCommand
        {
            get { return (ICommand)this.GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }
        

       
        private static void ItemsChanged(
            BindableObject bindable,
            ObservableCollection<T> oldValue,
            ObservableCollection<T> newValue)
        {
            var control = bindable as RepeaterView<T>;
            if (control == null)
                throw new Exception(
                    "Invalid bindable object passed to ReapterView::ItemsChanged expected a ReapterView<T> received a "
                    + bindable.GetType().Name);

            control._collectionSet(newValue);

      
           

        }

        void _collectionSet(ObservableCollection<T> newValue)
        {
            if (_collection != null)
            {
                _collection.CollectionChanged += _collection_CollectionChanged;
            }

            if (newValue != null)
            {
                _collection = newValue;
                _collection.CollectionChanged += _collection_CollectionChanged;
               
            }
        }

        private async void _collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                await _reset();
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems.OfType<T>())
                {
                    _removeItems(item);
                    await Task.Yield();
                }
            }

            if (e.NewItems!= null)
            {
                foreach (var item in e.NewItems.OfType<T>())
                {
                    _addItems(item);
                    await Task.Yield();
                }
            }
        }

        async Task _reset()
        {
            var children = Children.ToList();

            foreach (var child in children)
            {
                if (Children.Contains(child))
                {
                    Children.Remove(child);
                    await Task.Yield();
                }
            }
        }

        void _addItems(T item)
        {
            if (item == null)
            {
                return;
            }

            var view = _resolver.ResolveView(item);

            if (view == null)
            {
                return;
            }

            view.BindingContext = item;

            if (ItemClickCommand != null)
            {
                view.GestureRecognizers.Add(
                new TapGestureRecognizer { Command = ItemClickCommand, CommandParameter = item });
            }

            Children.Add(view);
        }

        void _removeItems(T item)
        {
            if (item == null)
            {
                return;
            }

            var child = Children.FirstOrDefault(_ => _.BindingContext == item);
            if (child != null)
            {
                Children.Remove(child);
            }
        }
    }
}