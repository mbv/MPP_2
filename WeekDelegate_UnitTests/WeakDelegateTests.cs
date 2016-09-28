using System;
using NUnit.Framework;
using WeekDelegate;

namespace WeekDelegate_UnitTests
{
    [TestFixture]
    public class WeakDelegateTests
    {
        private EventListener _eventListener;
        private WeakDelegate _weakDelegate;

        [Test]
        public void TestMemoryLeak()
        {
            EventSource eventSource = new EventSource();
            Allocate();

            long totalMemoryBeforeCollect = GC.GetTotalMemory(true);

            _eventListener = null;

            GC.Collect();
            long totalMemoryAfterCollect = GC.GetTotalMemory(true);

            Assert.AreEqual(true, totalMemoryBeforeCollect > totalMemoryAfterCollect);
            Console.WriteLine("Delta: {0}", totalMemoryBeforeCollect - totalMemoryAfterCollect);
        }

        [Test]
        public void TestMemoryLeakDefaultDelegate()
        {
            EventSource eventSource = new EventSource();
            EventListener eventListener = new EventListener();
            eventSource.FirstEventSource +=
                (Action<int>) eventListener.EventHandler;

            long totalMemoryBeforeCollect = GC.GetTotalMemory(true);

            eventListener = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long totalMemoryAfterCollect = GC.GetTotalMemory(true);

            Assert.AreEqual(true, totalMemoryBeforeCollect >= totalMemoryAfterCollect);
            Console.WriteLine("Delta: {0}", totalMemoryBeforeCollect - totalMemoryAfterCollect);
        }

        [Test]
        public void TestDeadWeekReferance()
        {
            EventSource eventSource = new EventSource();
            Allocate();

            eventSource.FirstEventSource += (Action<int>) _weakDelegate;

            _eventListener = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Assert.AreEqual(false, _weakDelegate.WeakReferenceToTarget.IsAlive);
        }

        private void Allocate()
        {
            _eventListener = new EventListener();
            _weakDelegate = new WeakDelegate((Action<int>) _eventListener.EventHandler);
        }
    }
}