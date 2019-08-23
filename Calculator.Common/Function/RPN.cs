using System;
using System.Collections.Generic;

namespace Calculator.Common.Function
{
    /// <summary>
    /// 逆ポーランド記法への変換を行うクラス
    /// </summary>
    public static class RPN
    {
        /// <summary>
        /// 指定された式を逆ポーランド記法に変換する
        /// </summary>
        /// <param name="formula">変換する中置記法の式</param>
        /// <returns>逆ポーランド記法に変換したトークンの列挙子</returns>
        public static IEnumerable<string> Convert(string formula)
        {
            if (formula == null)
            {
                throw new ArgumentNullException(nameof(formula));
            }

            return ConvertInternal();

            IEnumerable<string> ConvertInternal()
            {
                var operatorStack = new Stack<string>();

                foreach (string t in GetTokens(formula))
                {
                    string token = t;

                    //
                    // 単項演算子の判定
                    //
                    if (token == "+" || token == "-")
                    {
                        if (operatorStack.Count == 0)
                        {
                            token += "@";
                        }
                        else
                        {
                            switch (operatorStack.Peek())
                            {
                                case "(":
                                case "*":
                                case "/":
                                    token += "@";
                                    break;
                            }
                        }
                    }

                    //
                    // 優先度を判定し、変換処理を行う。
                    //
                    if (operatorStack.Count == 0 || GetTokenPriority(token) > GetTokenPriority(operatorStack.Peek()))
                    {
                        operatorStack.Push(token);
                    }
                    else if (token == ")")
                    {
                        if (operatorStack.Count == 0)
                        {
                            throw new FormatException();
                        }

                        while (operatorStack.Peek() != "(")
                        {
                            yield return operatorStack.Pop();

                            if (operatorStack.Count == 0)
                            {
                                throw new FormatException();
                            }
                        }
                        operatorStack.Pop();
                    }
                    else
                    {
                        while (operatorStack.Count > 0 && operatorStack.Peek() != "(" && GetTokenPriority(token) <= GetTokenPriority(operatorStack.Peek()))
                        {
                            yield return operatorStack.Pop();
                        }

                        operatorStack.Push(token);
                    }
                }

                while (operatorStack.Count > 0)
                {
                    string item = operatorStack.Pop();

                    if (item == "(")
                    {
                        throw new FormatException();
                    }

                    yield return item;
                }
            }
        }

        /// <summary>
        /// 入力式の指定位置のトークンを取得する
        /// </summary>
        /// <param name="formula">変換元の式</param>
        /// <returns>取得したトークンの列挙子</returns>
        private static IEnumerable<string> GetTokens(string formula)
        {
            int i = 0;

            while (i < formula.Length)
            {
                char formulaChar = formula[i];

                // 指定位置の文字が空白または制御文字の場合、スキップする
                if (char.IsWhiteSpace(formulaChar) || char.IsControl(formulaChar))
                {
                    i++;
                }
                // 括弧の場合
                else if (formulaChar == '(' || formulaChar == ')')
                {
                    yield return formulaChar.ToString();
                    i++;
                }
                // 指定位置の文字が数字の場合
                else if (char.IsDigit(formulaChar))
                {
                    int start = i;

                    do
                    {
                        i++;
                    }
                    while (i < formula.Length && char.IsDigit(formula[i]));

                    yield return formula[start..i];
                }
                // 指定位置の文字がアルファベットの場合
                else if (char.IsLetter(formulaChar) || char.IsSurrogate(formulaChar))
                {
                    int start = i;

                    do
                    {
                        i++;
                    }
                    while (i < formula.Length && (char.IsLetterOrDigit(formula[i]) || char.IsSurrogate(formula[i])));

                    yield return formula[start..i];
                }
                // 上記以外の場合
                else
                {
                    yield return formulaChar.ToString();
                    i++;
                }
            }
        }

        /// <summary>
        /// 指定されたトークンの優先度を取得する
        /// </summary>
        /// <param name="formula">優先度を求めるトークン文字列</param>
        /// <returns>指定されたトークンの優先度</returns>
        private static int GetTokenPriority(string token)
        {
            if (char.IsLetterOrDigit(token[0]) || char.IsSurrogate(token[0]))
            {
                return int.MaxValue;
            }
            else if (token == "(")
            {
                return 5;
            }
            else if (token == "+@" || token == "-@")
            {
                return 4;
            }
            else if (token == "*" || token == "/")
            {
                return 3;
            }
            else if (token == "+" || token == "-")
            {
                return 2;
            }
            else if (token == ")")
            {
                return 1;
            }
            else
            {
                throw new FormatException();
            }
        }
    }
}
