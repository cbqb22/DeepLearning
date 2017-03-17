using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeepLearning.Common.Command
{
    /// <summary>
    /// 同期処理のコマンドです。
    /// RelayCommandとの違いは、CommandManager.RequerySuggestedイベントによってCanExecute状態が自動再計算されることです。
    /// </summary>
    public class SyncRelayCommand<T> : ICommand
    {
        private WeakAction<T> _execute;
        private WeakFunc<T, bool> _canExecute;

        /// <summary>
        /// CanExecuteの状態変更イベント
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// コマンドが実行可能かどうかを返します。
        /// </summary>
        /// <param name="parameter">コマンドパラメータ</param>
        /// <returns>実行可能ならtrue</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null
                || (_canExecute.IsStatic || _canExecute.IsAlive)
                    && _canExecute.Execute();
        }

        /// <summary>
        /// コマンドを実行します。
        /// XAMLへのバインドではなく、コードから直接実行する場合はExecuteAsyncを使用してください。
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter)
                && _execute != null
                && (_execute.IsStatic || _execute.IsAlive))
            {
                _execute.Execute((T)parameter);
            }
        }

        /// <summary>
        /// CanExecute状態の再計算を行います。
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// 同期処理のコマンドを作成します。
        /// RelayCommandとの違いは、CommandManager.RequerySuggestedイベントによってCanExecute状態が自動再計算されることです。
        /// </summary>
        /// <param name="execute">処理内容</param>
        /// <param name="canExecute">実行可能判定</param>
        public SyncRelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = new WeakAction<T>(execute);

            if (canExecute != null)
            {
                _canExecute = new WeakFunc<T, bool>(canExecute);
            }
        }
    }
}
