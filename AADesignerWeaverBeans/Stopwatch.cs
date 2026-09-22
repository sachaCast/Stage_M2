// Decompiled with JetBrains decompiler
// Type: WComp.AADesigner.Stopwatch
// Assembly: AADesignerWeaverBeans, Version=3.2.2.1427, Culture=neutral, PublicKeyToken=null
// MVID: 586D5F29-3A68-4C9E-B737-87772B96A6F2
// Assembly location: C:\Users\sacha\OneDrive\Documents\beans copies\AADesignerWeaverBeans.dll

using System.Runtime.InteropServices;

namespace WComp.AADesigner
{
    public class Stopwatch
    {
        private long start = 0;
        private long stop = 0;
        private static double frequency = Stopwatch.getFrequency();

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private static double getFrequency()
        {
            long lpFrequency;
            Stopwatch.QueryPerformanceFrequency(out lpFrequency);
            return (double)lpFrequency;
        }

        public void Start() => Stopwatch.QueryPerformanceCounter(out this.start);

        public void Stop() => Stopwatch.QueryPerformanceCounter(out this.stop);

        public double Elapsed => (double)checked(this.stop - this.start) / Stopwatch.frequency;
    }
}