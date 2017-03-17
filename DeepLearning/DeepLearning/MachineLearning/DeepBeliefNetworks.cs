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
using DeepLearning.Const;
using System.Windows.Media.Imaging;
using DeepLearning.VewModel;
using System.Collections.ObjectModel;

namespace DeepLearning.MachineLearning
{
    public class DeepBeliefNetworks
    {
        private double[][] _inputs;
        private double[][] _outputs;
        private double[] _testInput;

        public void Init(double[][] inputs, double[][] outputs, double[] testInput)
        {
            _inputs = inputs;
            _outputs = outputs;
            _testInput = testInput;
        }


        public string Training(BitmapSource bitmapSource, MainWindowViewModel vm)
        {
            var result = string.Empty;

            // DBNの生成
            // 〇　〇　〇　〇　〇　〇　
            //     〇  〇  〇  〇
            //         〇  〇
            var network = new DeepBeliefNetwork(
                inputsCount: _inputs[0].Length,                                      // 入力層の次元
                hiddenNeurons: new int[] { 50, DeepLearningConst.OutputDimention }); // 隠れ層と出力層の次元

            // ネットワークの重みをガウス分布で初期化する
            new GaussianWeights(network).Randomize();
            network.UpdateVisibleWeights();

            // DBNの学習アルゴリズムの生成  5000回繰り返し入力
            var teacher = new BackPropagationLearning(network);
            for (int i = 0; i < 5000; i++)
            {
                var error = teacher.RunEpoch(_inputs, _outputs);
                vm.LearningProgressItems.Add(string.Format("Epoch:{0} Error:{1} \r\n", i, error));

                // 画像の切り替え
                if (i % 3 == 0)
                    vm.TrainingBitmapSource = vm.ImageSourceList[i % 10].BitmapSource;

            }

            // 重みの更新
            network.UpdateVisibleWeights();

            // 学習されたネットワークでテストデータが各クラスに属する確率を計算
            var output = network.Compute(_testInput);

            //  一番確率の高いクラスのインデックスを得る
            int imax; output.Max(out imax);

            result += string.Format("{0} \r\n", imax);

            // 結果出力
            int index = 0;
            output.ToList().ForEach(x => { result += string.Format("{0}: {1} \r\n", index++, x); });

            return result;
        }

        //public string Training()
        //{
        //    var result = string.Empty;


        //    // トレーニングデータ
        //    double[][] inputs = {
        //                            new double[] { 1, 1, 1, 0, 0, 0 },
        //                            new double[] { 1, 0, 1, 0, 0, 0 },
        //                            new double[] { 1, 1, 1, 0, 0, 0 },
        //                            new double[] { 0, 0, 1, 1, 1, 0 },
        //                            new double[] { 0, 0, 1, 1, 0, 0 },
        //                            new double[] { 0, 0, 1, 1, 1, 0 },
        //                        };
        //    double[][] outputs = {
        //                            new double[] { 1, 0 },
        //                            new double[] { 1, 0 },
        //                            new double[] { 1, 0 },
        //                            new double[] { 1, 0 },
        //                            new double[] { 0, 1 },
        //                            new double[] { 0, 1 },
        //                            new double[] { 0, 1 },
        //                        };

        //    // DBNの生成
        //    var network = new DeepBeliefNetwork(
        //        inputsCount: inputs.Length,         // 入力層の次元
        //        hiddenNeurons: new int[] { 4, 2 }); // 隠れ層と出力層の次元

        //    // ネットワークの重みをガウス分布で初期化する
        //    new GaussianWeights(network).Randomize();
        //    network.UpdateVisibleWeights();

        //    // DBNの学習アルゴリズムの生成  5000回繰り返し入力
        //    var teacher = new BackPropagationLearning(network);
        //    for (int i = 0; i < 5000; i++)
        //    {
        //        var error = teacher.RunEpoch(inputs, outputs);
        //        //result += string.Format("Error:{0} \r\n", error);

        //    }

        //    // 重みの更新
        //    network.UpdateVisibleWeights();

        //    // 学習されたネットワークでテストデータが各クラスに属する確率を計算
        //    double[] input = { 1, 1, 1, 1, 0, 0 };
        //    var output = network.Compute(input);

        //    //  一番確率の高いクラスのインデックスを得る
        //    int imax; output.Max(out imax);

        //    result += string.Format("{0} \r\n", imax);

        //    // 結果出力
        //    foreach (var o in output)
        //    {
        //        result += string.Format("{0} \r\n", o);
        //    }

        //    return result;
        //}
    }
}
