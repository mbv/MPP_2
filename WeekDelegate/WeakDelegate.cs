using System;

namespace WeekDelegate
{
    public class WeakDelegate
    {

        private Delegate _proxyDelegate;
        public WeakReference weakReferenceToTarget;

        public Delegate Week => _proxyDelegate;

        public WeakDelegate(Delegate eventHandler)
        {
            _proxyDelegate = eventHandler;
            weakReferenceToTarget = new WeakReference(eventHandler.Target);
        }
    }
}