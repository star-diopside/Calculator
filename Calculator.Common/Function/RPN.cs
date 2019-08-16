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
        /// <returns>逆ポーランド記法に変換したトークンの配列</returns>
        public static string[] Convert(string formula)
        {
            List<string> analysisList = new List<string>();
            Stack<string> operatorStack = new Stack<string>();
            string token;
            int index = 0;

            while ((token = getToken(formula, ref index)) != null)
            {
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
                if (operatorStack.Count == 0 || getTokenPriority(token) > getTokenPriority(operatorStack.Peek()))
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
                        analysisList.Add(operatorStack.Pop());

                        if (operatorStack.Count == 0)
                        {
                            throw new FormatException();
                        }
                    }
                    operatorStack.Pop();
                }
                else
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(" && getTokenPriority(token) <= getTokenPriority(operatorStack.Peek()))
                    {
                        analysisList.Add(operatorStack.Pop());
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

                analysisList.Add(item);
            }

            return analysisList.ToArray();
        }

        /// <summary>
        /// 入力式の指定位置のトークンを取得する
        /// </summary>
        /// <param name="formula">変換元の式</param>
        /// <param name="index">解析を開始するインデックスを指定する。解析後に次の解析開始位置に更新する。最後まで解析が完了した場合は -1 を返す。</param>
        /// <returns>取得したトークン。解析が完了済みの場合は null を返す。</returns>
        private static string getToken(string formula, ref int index)
        {
            // formula に null が指定された、または index にマイナス値または formula の文字列長以上の値が設定された場合、
            // index に -1 を設定し、nullを返す
            if (formula == null || index < 0 || index >= formula.Length)
            {
                index = -1;
                return null;
            }

            string token = null;

            // 入力式を解析し、トークンを取得する
            for (int i = index; i < formula.Length; i++)
            {
                if (char.IsWhiteSpace(formula, i) || char.IsControl(formula, i))
                {
                    // 指定位置の文字が空白または制御文字の場合、スキップする
                    continue;
                }
                else if (formula[i] == '(' || formula[i] == ')')
                {
                    // 括弧の場合
                    int startIndex = i;
                    index = i + 1;
                    return formula.Substring(startIndex, 1);
                }
                else if (char.IsDigit(formula, i))
                {
                    // 指定位置の文字が数字の場合
                    int startIndex = i;
                    int count = 1;

                    while (i + count < formula.Length && char.IsDigit(formula, i + count))
                    {
                        count++;
                    }

                    index = i + count;
                    return formula.Substring(startIndex, count);
                }
                else if (char.IsLetter(formula, i) || char.IsSurrogate(formula, i))
                {
                    // 指定位置の文字がアルファベットの場合
                    int startIndex = i;
                    int count = 1;

                    while (i + count < formula.Length && (char.IsLetterOrDigit(formula, i + count) || char.IsSurrogate(formula, i + count)))
                    {
                        count++;
                    }

                    index = i + count;
                    return formula.Substring(startIndex, count);
                }
                else
                {
                    // 上記以外の場合
                    int startIndex = i;
                    index = i + 1;
                    return formula.Substring(startIndex, 1);
                }
            }

            return token;
        }

        /// <summary>
        /// 指定されたトークンの優先度を取得する
        /// </summary>
        /// <param name="formula">優先度を求めるトークン文字列</param>
        /// <returns>指定されたトークンの優先度</returns>
        private static int getTokenPriority(string token)
        {
            if (char.IsLetterOrDigit(token, 0) || char.IsSurrogate(token, 0))
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
