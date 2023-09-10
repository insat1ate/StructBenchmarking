using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using System.Windows.Forms;

namespace StructBenchmarking
{
    public class SpecialCreate : ITask
    {
        public void Run()
        {
            new string('a', 1000);
        }
    }
    public class BuilderCreate : ITask
    {
        private StringBuilder builder = new StringBuilder();
        public void Run()
        {

            for (int i = 0; i < 1000; i++) builder.Append('a');

            builder.ToString();
        }
    }
    public class Benchmark : IBenchmark
    {
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            GC.Collect();                   // Эти две строчки нужны, чтобы уменьшить вероятность того,
            GC.WaitForPendingFinalizers();  // что Garbadge Collector вызовется в середине измерений
                                            // и как-то повлияет на них.
            task.Run();
            var timer = new Stopwatch();
            timer.Restart();
            for (int i = 0; i < repetitionCount; i++)
            {
                task.Run();
            }
            timer.Stop();



            return timer.Elapsed.TotalMilliseconds / repetitionCount;

        }
    }

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var builderCreate = new BuilderCreate();
            var specialCreate = new SpecialCreate();
            var benchmark = new Benchmark();
            var ansBuilderCreate = benchmark.MeasureDurationInMs(builderCreate, 1000);
            var ansSpecialCreate = benchmark.MeasureDurationInMs(specialCreate, 1000);

            Assert.Less(ansSpecialCreate, ansBuilderCreate);
        }
    }
}