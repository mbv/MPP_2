using System;

namespace WeekDelegate
{
    public class WeakDelegate
    {

        private Delegate _proxyDelegate;

        public Delegate Week => _proxyDelegate;

        public WeakDelegate(Delegate eventHandler)
        {
            _proxyDelegate = eventHandler;
        }
    }
}