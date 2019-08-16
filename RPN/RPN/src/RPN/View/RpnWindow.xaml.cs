using System;
using System.Text;
using System.Windows;
using RPN.Function;

namespace RPN.View
{
    /// <summary>
    /// RpnWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class RpnWindow : Window
    {
        public RpnWindow()
        {
            InitializeComponent();
        }

        private void AnalysisEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] result = ReversePolishNotation.Convert(textInput.Text);

                StringBuilder sb = new StringBuilder();
                foreach (string item in result)
                {
                    sb.Append(item).Append(' ');
                }
                if (sb.Length >= 1)
                {
                    sb.Length -= 1;
                }

                textTransfer.Text = sb.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show(this, "数式の書式が不正です。", "解析エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
