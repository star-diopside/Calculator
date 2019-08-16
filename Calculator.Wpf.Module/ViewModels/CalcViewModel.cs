using Calculator.Wpf.Module.Models;
using Prism.Commands;
using Reactive.Bindings;
using System.Windows.Input;

namespace Calculator.Wpf.Module.ViewModels
{
    class CalcViewModel
    {
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

        public CalcViewModel(CalcModel model)
        {
            _model = model;
            AnalysisCommand = new DelegateCommand(Analysis);
            CalculateCommand = new DelegateCommand(Calculate);
        }

        private void Analysis()
        {
            _model.Analysis();
        }

        private void Calculate()
        {
        }
    }
}
