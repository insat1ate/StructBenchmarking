using System.Collections.Generic;

namespace StructBenchmarking
{
    interface IExperiments
    {
        List<ExperimentResult> RunExperiment();
    }

    public abstract class AbstractExperiment : IExperiments
    {
        public IBenchmark bench { get; set; }

        public AbstractExperiment(IBenchmark bench)
        {
            this.bench = bench;
        }
        public abstract ITask GetITask(int size);
        public List<ExperimentResult> RunExperiment()
        {
            var listExperimentResult = new List<ExperimentResult>();

            foreach (var field in Constants.FieldCounts)
            {
                listExperimentResult.Add(new ExperimentResult(field, bench.MeasureDurationInMs(GetITask(field), 100)));
            }
            return listExperimentResult;
        }
    }
    public class StructArrayCreation : AbstractExperiment
    {
        public StructArrayCreation(IBenchmark bench) : base(bench)
        {
        }

        public override ITask GetITask(int size)
        {
            return new StructArrayCreationTask(size);
        }
    }
    public class ClassArrayCreation : AbstractExperiment
    {
        public ClassArrayCreation(IBenchmark bench) : base(bench)
        {
        }

        public override ITask GetITask(int size)
        {
            return new ClassArrayCreationTask(size);
        }
    }

    public class ClassMethodCreation : AbstractExperiment
    {
        public ClassMethodCreation(IBenchmark bench) : base(bench)
        {
        }

        public override ITask GetITask(int size)
        {
            return new MethodCallWithClassArgumentTask(size);
        }
    }

    public class StructMethodCreation : AbstractExperiment
    {
        public StructMethodCreation(IBenchmark bench) : base(bench)
        {
        }

        public override ITask GetITask(int size)
        {
            return new MethodCallWithStructArgumentTask(size);
        }
    }

    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionsCount)
        {


            var classesTimes = new ClassArrayCreation(benchmark).RunExperiment();
            var structuresTimes = new StructArrayCreation(benchmark).RunExperiment();




            return new ChartData
            {
                Title = "Create array",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new ClassMethodCreation(benchmark).RunExperiment();
            var structuresTimes = new StructMethodCreation(benchmark).RunExperiment();

            return new ChartData
            {
                Title = "Call method with argument",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }
    }
}