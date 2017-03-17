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
    public class SyncRelayCommand : ICommand
    {
        private WeakAction _execute;
        private WeakFunc<bool> _canExecute;

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
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter)
                && _execute != null
                && (_execute.IsStatic || _execute.IsAlive))
            {
                _execute.Execute();
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
        public SyncRelayCommand(Action execute, Func<bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = new WeakAction(execute);

            if (canExecute != null)
            {
                _canExecute = new WeakFunc<bool>(canExecute);
            }
        }
    }
}
