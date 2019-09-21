using Calculator.Wpf.Module.Models;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Reactive.Bindings;
using System;
using System.Windows;
using System.Windows.Input;

namespace Calculator.Wpf.Module.ViewModels
{
    class CalcViewModel
    {
        private readonly ILogger<CalcViewModel> _logger;
        private readonly CalcModel _model;

        /// <summary>入力式を示すプロパティを取得する。</summary>
        public ReactivePropertySlim<string> Input => _model.Input;

        /// <summary>逆ポーランド記法への変換式を示すプロパティを取得する。</summary>
        public ReactivePropertySlim<string> Transfer => _model.Transfer;

        /// <summary>計算結果を示すプロパティを取得する。</summary>
        public ReactivePropertySlim<string> Result => _model.Result;

        /// <summary>解析コマンドを取得する。</summary>
        public ICommand AnalysisCommand { get; }

        /// <summary>計算コマンドを取得する。</summary>
        public ICommand CalculateCommand { get; }

        public CalcViewModel(ILogger<CalcViewModel> logger, CalcModel model)
        {
            _logger = logger;
            _model = model;
            AnalysisCommand = new DelegateCommand(Analysis);
            CalculateCommand = new DelegateCommand(Calculate);
        }

        private void Analysis()
        {
            try
            {
                _model.Analysis();
            }
            catch (FormatException e)
            {
                _logger.LogError(e, e.Message);
                MessageBox.Show("数式の書式が不正です。", "解析エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Calculate()
        {
            _model.Calculate();
        }
    }
}
