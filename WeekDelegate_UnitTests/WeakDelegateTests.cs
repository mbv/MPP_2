using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeekDelegate;

namespace WeekDelegate_UnitTests
{
    [TestClass]
    public class WeakDelegateTests
    {
        [TestMethod]
        public void TestMemoryLeak()
        {
            EventSource eventSource = new EventSource();
            EventListener eventListener = new EventListener();
            eventSource.FirstEventSource +=
                (Action<int>) new WeakDelegate((Action<int>) eventListener.EventHandler);

            long totalMemoryBeforeCollect = GC.GetTotalMemory(true);

            eventListener = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
            GC.Collect();

            long totalMemoryAfterCollect = GC.GetTotalMemory(true);

            Assert.AreEqual(true, totalMemoryBeforeCollect > totalMemoryAfterCollect);
            Console.WriteLine("Delta: {0}", totalMemoryBeforeCollect - totalMemoryAfterCollect);
        }

        [TestMethod]
        public void TestMemoryLeakDefaultDelegate()
        {
            EventSource eventSource = new EventSource();
            EventListener eventListener = new EventListener();
            eventSource.FirstEventSource +=
                (Action<int>)eventListener.EventHandler;

            long totalMemoryBeforeCollect = GC.GetTotalMemory(true);

            eventListener = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
            GC.Collect();

            long totalMemoryAfterCollect = GC.GetTotalMemory(true);

            Assert.AreEqual(true, totalMemoryBeforeCollect >= totalMemoryAfterCollect);
            Console.WriteLine("Delta: {0}", totalMemoryBeforeCollect-totalMemoryAfterCollect);
        }

        [TestMethod]
        public void TestDeadWeekReferance()
        {
            EventSource eventSource = new EventSource();
            EventListener eventListener = new EventListener();
            WeakDelegate weakDelegate = new WeakDelegate((Action<int>) eventListener.EventHandler);
            eventSource.FirstEventSource += (Action<int>) weakDelegate;

            eventListener = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
            GC.Collect();

            Assert.AreEqual(false, weakDelegate.WeakReferenceToTarget.IsAlive);
        }
    }
}