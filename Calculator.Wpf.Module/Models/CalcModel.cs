using Calculator.Common.Function;
using Reactive.Bindings;
using System;

namespace Calculator.Wpf.Module.Models
{
    class CalcModel
    {
        /// <summary>入力式を示すプロパティを取得する。</summary>
        public ReactivePropertySlim<string> Input { get; } = new ReactivePropertySlim<string>(string.Empty);

        /// <summary>逆ポーランド記法への変換式を示すプロパティを取得する。</summary>
        public ReactivePropertySlim<string> Transfer { get; } = new ReactivePropertySlim<string>();

        /// <summary>計算結果を示すプロパティを取得する。</summary>
        public ReactivePropertySlim<string> Result { get; } = new ReactivePropertySlim<string>();

        public void Analysis()
        {
            Transfer.Value = string.Join(' ', ReversePolishNotation.Convert(Input.Value));
        }

        public void Calculate()
        {
            throw new NotImplementedException();
        }
    }
}
