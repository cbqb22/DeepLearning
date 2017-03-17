using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Neuro;
using Accord.Neuro.Networks;
using Accord.Neuro.Learning;
using Accord.Neuro.ActivationFunctions;
using Accord.Math;
using Accord.Statistics;

namespace DeepLearning.MachineLearning
{
    public class RestrictedBoltzmannMachineWork
    {

        public string Training()
        {

            var result = string.Empty;

            // トレーニングデータ
            double[][] inputs = {
                new double[] { 1, 1, 1, 0, 0, 0 },
                new double[] { 1, 0, 1, 0, 0, 0 },
                new double[] { 1, 1, 1, 0, 0, 0 },
                new double[] { 0, 0, 1, 1, 1, 0 },
                new double[] { 0, 0, 1, 1, 0, 0 },
                new double[] { 0, 0, 1, 1, 1, 0 },
            };
            double[][] outputs = {
                new double[] { 1, 0 },
                new double[] { 1, 0 },
                new double[] { 1, 0 },
                new double[] { 0, 1 },
                new double[] { 0, 1 },
                new double[] { 0, 1 },
            };
            // RBMの生成
            var rbm = new RestrictedBoltzmannMachine(
                inputsCount: 6,
                hiddenNeurons: 2);

            // トレーニングデータで学習
            var teacher = new ContrastiveDivergenceLearning(rbm);
            for (int i = 0; i < 5000; i++)
                teacher.RunEpoch(inputs);

            // テストデータ
            double[] input = { 1, 1, 1, 1, 0, 0 };

            // 学習されたネットワークで各クラスに属する確率を計算
            var output = rbm.Compute(input);

            //  一番確率の高いクラスのインデックスを得る
            int imax; output.Max(out imax);

            result += string.Format("{0} \r\n", imax);

            // 結果出力
            foreach (var o in output)
            {
                result += string.Format("{0} \r\n", o);
            }

            return result;
        }

        
    }
}

