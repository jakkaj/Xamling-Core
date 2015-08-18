using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Messages.XamlingMessenger
{
    public class XMessenger
    {
        public static XMessenger Default { get; private set; }


        private readonly Dictionary<Type, List<ActionRegistration>> _registrations;

        static XMessenger()
        {
            Default = new XMessenger();
        }

        public XMessenger()
        {
            _registrations = new Dictionary<Type, List<ActionRegistration>>();
        }

        public void Register<T>(object registrant, Action<object> callback) where T : XMessage
        {
            var registration = new ActionRegistration(registrant, callback);

            var t = typeof(T);
            _event.WaitOne(200);
            _event.Reset();
            _get(t).Add(registration);
            _event.Set();
        }

        public void Register<T>(object registrant, Action callback) where T : XMessage
        {
            var registration = new ActionRegistration(registrant, callback);

            var t = typeof(T);
            _event.WaitOne(200);
            _event.Reset();
            _get(t).Add(registration);
            _event.Set();
        }

        public bool IsRegistered(object t)
        {
            return _registrations.Select(item => item.Value).Any(v => v.Any(vItem => vItem.Registrant == t));
        }

        List<ActionRegistration> _get(Type t)
        {
            if (!_registrations.ContainsKey(t))
            {
                _registrations.Add(t, new List<ActionRegistration>());
            }

            var l = _registrations[t];

            if (l != null) return l;

            l = new List<ActionRegistration>();
            _registrations[t] = l;

            return l;
        }


        ManualResetEvent _event = new ManualResetEvent(true);
        public void Send<T>(T message) where T : XMessage
        {
            var t = message.GetType();

            if (!_registrations.ContainsKey(t)) //nothing has registered for this message
            {
                return;
            }

            var callList = _registrations[t];

            if (callList == null) return;

            _event.WaitOne(200);
            _event.Reset();
            //tries to call the message, if it fails then it removes that message subscription. 
            var itemsToRemove = (from item in callList let callResult = item.Action(message) where !callResult select item).ToList();

            

            foreach (var item in itemsToRemove)
            {
                callList.Remove(item);
            }

            _event.Set();


        }

        public void Unregister(object registrant)
        {
            _event.WaitOne(200);
            _event.Reset();
            var removeList = (from item in _registrations from action in item.Value where action.Registrant == registrant select action).ToList();
            foreach (var item in removeList)
            {
                var closeItem = item;
                closeItem.Dispose();
                foreach (var registration in _registrations.Where(registration => registration.Value.Contains(closeItem)))
                {
                    registration.Value.Remove(closeItem);
                }
            }
            _event.Set();
        }

        protected class ActionRegistration : IDisposable
        {
            private Action<object> _callbackReference;
            private Action _callbackReferencePlain;
            private object _registrant;

            public ActionRegistration(object registrant, Action<object> callback)
            {
                _callbackReference = callback;
                _registrant = registrant;
            }

            public ActionRegistration(object registrant, Action callback)
            {
                _callbackReferencePlain = callback;
                _registrant = registrant;
            }

            public bool Action(object message)
            {
                try
                {
                    if (_callbackReference == null && _callbackReferencePlain == null)
                    {
                        return false;
                    }

                    Task.Run(() =>
                    {
                        if (_callbackReference != null)
                        {
                            _callbackReference(message);
                        }
                        if (_callbackReferencePlain != null)
                        {
                            _callbackReferencePlain();
                        }
                    });
                    return true;
                }
                catch
                {
                }
                return false;
            }

            public Action<object> CallbackReference
            {
                get { return _callbackReference; }
            }

            public Action CallbackReferencePlain
            {
                get { return _callbackReferencePlain; }
            }

            public object Registrant
            {
                get { return _registrant; }
            }

            public void Dispose()
            {
                _callbackReference = null;
                _registrant = null;
            }
        }
    }
    public static class XMessengerRegisterExtension
    {
        public static void Register<T>(this object registrant, Action<object> callback) where T : XMessage
        {
            XMessenger.Default.Register<T>(registrant, callback);
        }

        public static void Register<T>(this object registrant, Action callback) where T : XMessage
        {
            XMessenger.Default.Register<T>(registrant, callback);
        }
    }
}
