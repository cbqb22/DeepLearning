using DeepLearning.Common.Command;
using DeepLearning.MNIST;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using DeepLearning.Common.Draw;
using DeepLearning.MachineLearning;
using System.Collections.Generic;
using DeepLearning.Const;
using System.Windows.Data;

namespace DeepLearning.VewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region フィールド


        private string pixelFile = @"C:\Mnist\train-images.idx3-ubyte";
        private string labelFile = @"C:\Mnist\train-labels.idx1-ubyte";

        #endregion
        #region プロパティ

        private ObservableCollection<DigitImage> _imageSourceList;
        public ObservableCollection<DigitImage> ImageSourceList { get { return _imageSourceList; } set { Set(ref _imageSourceList, value); } }

        private string _result;
        public string Result { get { return _result; } set { Set(ref _result, value); } }

        private string _input;
        public string Input { get { return _input; } set { Set(ref _input, value); } }

        private bool _isLoading = true;
        public bool IsLoading { get => _isLoading; set { Set(ref _isLoading, value); } }

        private bool _isInitTraining = false;
        public bool IsInitTraining { get => _isInitTraining; set { Set(ref _isInitTraining, value); } }

        private bool _isRunning = false;
        public bool IsRunning { get => _isRunning; set { Set(ref _isRunning, value); } }

        private ObservableCollection<string> _learningProgressItems = new ObservableCollection<string>();
        public ObservableCollection<string> LearningProgressItems { get => _learningProgressItems; set { Set(ref _learningProgressItems, value); } }

        private Dictionary<byte, List<Bitmap>> bitmapList = new Dictionary<byte, List<Bitmap>>();

        private BitmapSource _trainingBitmapSource;
        public BitmapSource TrainingBitmapSource { get => _trainingBitmapSource; set { Set(ref _trainingBitmapSource, value); } }


        private int _selectIndex;
        public int SelectIndex { get => _selectIndex; set { Set(ref _selectIndex, value); } }

        #endregion
        #region コマンド

        public ICommand TrainingCommand { get; private set; }
        public ICommand DecisionCommand { get; private set; }
        public ICommand LoadedCommand { get; private set; }
        public ICommand StopCommand { get; private set; }

        #endregion
        #region コンストラクタ

        public MainWindowViewModel()
        {
            TrainingCommand = new AsyncRelayCommand(Training);
            DecisionCommand = new RelayCommand(Decision);
            LoadedCommand = new AsyncRelayCommand(Load);
            StopCommand = new AsyncRelayCommand(Stop);

            // 他スレッドからコレクション追加を可能にする
            BindingOperations.EnableCollectionSynchronization(this.LearningProgressItems, new object());
        }

        #endregion
        #region パブリックメソッド

        #endregion
        #region プライベートメソッド

        private async Task Load()
        {
            var collection = await Task.Run<ObservableCollection<DigitImage>>(() => { return Init(); });
            ImageSourceList = collection;
            IsLoading = false;
        }

        private async Task Stop()
        {
            await Task.Run(() => { });
        }


        private ObservableCollection<DigitImage> Init()
        {
            var ret = new ObservableCollection<DigitImage>();

            // イメージリスト作成
            var images = DigitImageHelper.LoadData(pixelFile, labelFile);
            images.ToList().Skip(59850).ToList().ForEach(x =>
            {
                using (var bitmap = DigitImageHelper.MakeBitmap(x, 1))
                {
                    //System.Diagnostics.Debug.WriteLine(bitmap.GetHashCode());

                    if (!bitmapList.ContainsKey(x.label))
                        bitmapList.Add(x.label, new List<Bitmap>());
                    bitmapList[x.label].Add(bitmap);
                    x.imageBytes = BitmapConveter.ConvertImageToBmpBytes(bitmap);
                    x.BitmapSource = DigitImageHelper.BitmapToImageSource(bitmap);
                    ret.Add(x);
                }
            });

            return ret;
        }


        /// <summary>
        /// トレーニング実行
        /// </summary>
        /// <returns></returns>
        private async Task Training()
        {
            if (SelectIndex < 0)
                return;

            var selectItem = ImageSourceList[SelectIndex];


            IsRunning = true;
            Result = await Task.Run<string>(() =>
            {
                IsInitTraining = true;
                var inputs = new List<double[]>();
                var outputs = new List<double[]>();
                double[] testInput = null;

                int counter = 0;

                // トレーニングイメージセット
                ImageSourceList.ToList().ForEach(x =>
                {

                    var dArray = BitmapConveter.ByteArrayToDoubleArray(x.imageBytes);
                    inputs.Add(dArray);
                    if (counter == SelectIndex)
                    {
                        testInput = dArray;
                        Input = x.label.ToString();
                    }
                    var output = Enumerable.Range(0, DeepLearningConst.OutputDimention)
                              .ToList()
                              .Select(index =>
                              {
                                  if (index == x.label)
                                      return 1d;
                                  else
                                      return 0d;
                              }).ToArray();
                    outputs.Add(output);

                    counter++;

                });


                //// トレーニングイメージセット
                //bitmapList.ToList().ForEach(x =>
                //{
                //    counter++;
                //    x.Value.ForEach(bitmap =>
                //    {
                //        var arr = BitmapConveter.ByteArrayToDoubleArray(BitmapConveter.ConvertImageToBmpBytes(bitmap));
                //        inputs.Add(arr);
                //        if (counter == 4)
                //        {
                //            testInput = arr;
                //            Input = x.Key.ToString();
                //        }
                //        var output = Enumerable.Range(0, DeepLearningConst.OutputDimention)
                //                  .ToList()
                //                  .Select(index =>
                //                    {
                //                        if (index == x.Key)
                //                            return 1d;
                //                        else
                //                            return 0d;
                //                    }).ToArray();
                //        outputs.Add(output);
                //    });
                //});

                IsInitTraining = false;

                // トレーニング
                DeepBeliefNetworks dbn = new DeepBeliefNetworks();
                dbn.Init(inputs.ToArray(), outputs.ToArray(), testInput.ToArray());


                counter++;

                return dbn.Training(TrainingBitmapSource, this);
            });

            IsRunning = false;
        }


        private void Decision()
        {
        }

        #endregion
    }
}
