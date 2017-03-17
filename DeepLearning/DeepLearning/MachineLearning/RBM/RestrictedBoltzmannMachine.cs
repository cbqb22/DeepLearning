using System;
using System.Threading.Tasks;

namespace DeepLearning.MachineLearning.RBM
{
    class RestrictedBoltzmannMachine
    {
        public SymmetricConnection[][] Connections;
        public VisibleNeuron[] VisibleNeurons;
        public HiddenNeuron[] HiddenNeurons;

        public RestrictedBoltzmannMachine(int visibleNeuronCount, int hiddenNeuronCount, Random random) :
            this(SymmetricConnection.CreateRandomWeights(random, visibleNeuronCount, hiddenNeuronCount), new double[visibleNeuronCount], new double[hiddenNeuronCount], random)
        {
        }

        public RestrictedBoltzmannMachine(double[][] weights, double[] visibleBiases, double[] hiddenBiases, Random random)
        {
            this.VisibleNeurons = Neuron.CreateNeurons<VisibleNeuron>(visibleBiases);
            this.HiddenNeurons = Neuron.CreateNeurons<HiddenNeuron>(hiddenBiases);
            this.Connections = SymmetricConnection.CreateConnections(weights, VisibleNeurons, HiddenNeurons);
            Neuron.WireConnections(this.Connections);

            foreach (var neuron in this.HiddenNeurons)
            {
                neuron.Random = new Random(random.Next());
            }
        }

        public void SetVisibleNeuronValues(double[] visibleValues)
        {
            for (int i = 0; i < this.VisibleNeurons.Length; i++)
            {
                this.VisibleNeurons[i].Value = visibleValues[i];
            }
        }

        public void LearnFromData(double learningRate, int freeAssociationStepCount = 1)
        {
            Wake(learningRate);
            Sleep(learningRate, freeAssociationStepCount);
            EndLearning();
        }

        public void Wake(double learningRate)
        {
            UpdateHiddenNeurons();
            learn(learningRate);
        }

        public void UpdateVisibleNeurons()
        {
            updateNeurons(this.VisibleNeurons);
        }

        public void UpdateHiddenNeurons()
        {
            updateNeurons(this.HiddenNeurons);
        }

        private void updateNeurons(Neuron[] neurons)
        {
            Parallel.ForEach(neurons, neuron => neuron.Update());
        }

        private void learn(double learningRate)
        {
            foreach (var connectionRow in Connections)
            {
                foreach (var connection in connectionRow)
                {
                    connection.Learn(learningRate);
                }
            }

            foreach (var neuron in this.VisibleNeurons)
            {
                neuron.Learn(learningRate);
            }

            foreach (var neuron in this.HiddenNeurons)
            {
                neuron.Learn(learningRate);
            }
        }

        public void Sleep(double learningRate, int freeAssociationStepCount)
        {
            doFreeAssociation(freeAssociationStepCount);
            learn(-learningRate);
        }

        //Gibbs sampling
        private void doFreeAssociation(int freeAssociationStepCount)
        {
            for (int step = 0; step < freeAssociationStepCount; step++)
            {
                UpdateVisibleNeurons();
                UpdateHiddenNeurons();
            }
        }

        public void EndLearning()
        {
            foreach (var connectionRow in Connections)
            {
                foreach (var connection in connectionRow)
                {
                    connection.EndLearning();
                }
            }

            foreach (var neuron in this.VisibleNeurons)
            {
                neuron.EndLearning();
            }

            foreach (var neuron in this.HiddenNeurons)
            {
                neuron.EndLearning();
            }
        }

        public void Associate()
        {
            UpdateHiddenNeurons();
            UpdateVisibleNeurons();
        }
    }
}