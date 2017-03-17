using System;

namespace DeepLearning.MachineLearning.RBM
{
    public class Synapse
    {
        public Neuron SourceNeuron;
        public SymmetricConnection Connection;
    }

    public class SymmetricConnection
    {
        public double Weight;
        public double DeltaWeight;
        public VisibleNeuron VisibleNeuron;
        public HiddenNeuron HiddenNeuron;

        public void Learn(double learningRate)
        {
            this.DeltaWeight +=
                learningRate * VisibleNeuron.Value * HiddenNeuron.Probability;
        }

        public void EndLearning()
        {
            this.Weight += this.DeltaWeight;
            this.DeltaWeight = 0;
        }

        public static double[][] CreateRandomWeights(Random random, int visibleNeuronCount, int hiddenNeuronCount)
        {
            var result = createJaggedArray<double>(visibleNeuronCount, hiddenNeuronCount);

            double a = 1.0 / visibleNeuronCount;

            for (int i = 0; i < visibleNeuronCount; i++)
            {
                for (int j = 0; j < hiddenNeuronCount; j++)
                {
                    result[i][j] = uniform(random, -a, a);
                }
            }

            return result;
        }

        private static T[][] createJaggedArray<T>(int visibleNeuronCount, int hiddenNeuronCount)
        {
            var result = new T[visibleNeuronCount][];

            for (int i = 0; i < visibleNeuronCount; i++)
            {
                result[i] = new T[hiddenNeuronCount];
            }

            return result;
        }

        private static double uniform(Random random, double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        public static SymmetricConnection[][] CreateConnections(
            double[][] weights,
            VisibleNeuron[] visibleNeurons,
            HiddenNeuron[] hiddenNeurons)
        {
            var result = createJaggedArray<SymmetricConnection>(visibleNeurons.Length, hiddenNeurons.Length);

            for (int i = 0; i < visibleNeurons.Length; i++)
            {
                for (int j = 0; j < hiddenNeurons.Length; j++)
                {
                    SymmetricConnection connection = new SymmetricConnection();
                    connection.Weight = weights[i][j];
                    connection.VisibleNeuron = visibleNeurons[i];
                    connection.HiddenNeuron = hiddenNeurons[j];
                    result[i][j] = connection;
                }
            }

            return result;
        }
    }
}