using Calculator.Common.Function;
using Reactive.Bindings;
using System;
using System.Windows;

namespace Calculator.Wpf.Module.Models
{
    class CalcModel
    {
        /// <summary>入力式を示すプロパティを取得する。</summary>
        public ReactivePropertySlim<string> Input { get; } = new ReactivePropertySlim<string>();

        /// <summary>逆ポーランド記法への変換式を示すプロパティを取得する。</summary>
        public ReactivePropertySlim<string> Transfer { get; } = new ReactivePropertySlim<string>();

        /// <summary>計算結果を示すプロパティを取得する。</summary>
        public ReactivePropertySlim<string> Result { get; } = new ReactivePropertySlim<string>();

        public void Analysis()
        {
            try
            {
                Transfer.Value = string.Join(' ', RPN.Convert(Input.Value));
            }
            catch (FormatException)
            {
                MessageBox.Show("数式の書式が不正です。", "解析エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
