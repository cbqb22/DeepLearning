using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepLearning.MachineLearning.RBM
{
    public abstract class Neuron
    {
        public double Value;
        public double Bias;
        public double DeltaBias;
        public List<Synapse> Synapses = new List<Synapse>();

        public abstract void Update();
        public abstract void Learn(double learningRate);

        public void EndLearning()
        {
            this.Bias += this.DeltaBias;
            this.DeltaBias = 0;
        }

        public static T[] CreateNeurons<T>(double[] biases)
            where T : Neuron, new()
        {
            T[] result = new T[biases.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new T { Bias = biases[i] };
            }

            return result;
        }

        public static void WireConnections(SymmetricConnection[][] connections)
        {
            foreach (var connectionRow in connections)
            {
                foreach (var connection in connectionRow)
                {
                    Synapse hiddenConnection = new Synapse();
                    hiddenConnection.Connection = connection;
                    hiddenConnection.SourceNeuron = connection.VisibleNeuron;
                    connection.HiddenNeuron.Synapses.Add(hiddenConnection);

                    Synapse visibleConnection = new Synapse();
                    visibleConnection.Connection = connection;
                    visibleConnection.SourceNeuron = connection.HiddenNeuron;
                    connection.VisibleNeuron.Synapses.Add(visibleConnection);
                }
            }
        }

        protected double GetInputFromSourceNeurons()
        {
            return Synapses.Sum(s => s.Connection.Weight * s.SourceNeuron.Value) + Bias;
        }

        protected static double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }
    }

    public class VisibleNeuron : Neuron
    {
        public override void Update()
        {
            this.Value = Sigmoid(GetInputFromSourceNeurons());
        }

        public override void Learn(double learningRate)
        {
            this.DeltaBias += learningRate * this.Value;
        }
    }

    public class HiddenNeuron : Neuron
    {
        public double Probability;
        public Random Random;

        public override void Learn(double learningRate)
        {
            this.DeltaBias += learningRate * this.Probability;
        }

        public override void Update()
        {
            this.Probability = Sigmoid(GetInputFromSourceNeurons());
            this.Value = nextBool(Random, this.Probability) ? 1 : 0;
        }

        private static bool nextBool(Random random, double rate)
        {
            if (rate < 0 || 1 < rate) return false;
            return random.NextDouble() < rate;
        }
    }
}