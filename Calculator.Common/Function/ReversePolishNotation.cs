namespace Calculator.Common.Function;

/// <summary>
/// 逆ポーランド記法への変換を行うクラス
/// </summary>
public static class ReversePolishNotation
{
    /// <summary>
    /// 指定された式を逆ポーランド記法に変換する
    /// </summary>
    /// <param name="formula">変換する中置記法の式</param>
    /// <returns>逆ポーランド記法に変換したトークンの列挙子</returns>
    public static IEnumerable<string> Convert(string formula)
    {
        ArgumentNullException.ThrowIfNull(formula);

        return ConvertInternal();

        IEnumerable<string> ConvertInternal()
        {
            var tokenStack = new Stack<string>();

            foreach (string t in GetTokens(formula))
            {
                string token = t;
                bool stackNotEmpty = tokenStack.TryPeek(out string? peekToken);

                //
                // 単項演算子の判定
                //
                if (token == "+" || token == "-")
                {
                    if (stackNotEmpty)
                    {
                        switch (peekToken)
                        {
                            case "+":
                            case "-":
                            case "*":
                            case "/":
                            case "(":
                                token += "@";
                                break;

                            case "+@":
                            case "-@":
                                throw new FormatException();
                        }
                    }
                    else
                    {
                        token += "@";
                    }
                }

                //
                // 優先度を判定し、変換処理を行う。
                //
                if (!stackNotEmpty || GetTokenPriority(token) > GetTokenPriority(peekToken))
                {
                    tokenStack.Push(token);
                }
                else if (char.IsLetterOrDigit(token[0]) && char.IsLetterOrDigit(peekToken[0]))
                {
                    throw new FormatException();
                }
                else if (token == ")")
                {
                    if (!stackNotEmpty)
                    {
                        throw new FormatException();
                    }

                    while (peekToken != "(")
                    {
                        yield return tokenStack.Pop();

                        if (!tokenStack.TryPeek(out peekToken))
                        {
                            throw new FormatException();
                        }
                    }
                    tokenStack.Pop();
                }
                else
                {
                    while (tokenStack.TryPeek(out peekToken) && peekToken != "(" && GetTokenPriority(token) <= GetTokenPriority(peekToken))
                    {
                        yield return tokenStack.Pop();
                    }

                    tokenStack.Push(token);
                }
            }

            while (tokenStack.Count > 0)
            {
                string item = tokenStack.Pop();

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
            // 演算子の場合
            else if (formulaChar == '+' || formulaChar == '-' || formulaChar == '*' || formulaChar == '/' ||
                     formulaChar == '(' || formulaChar == ')')
            {
                yield return formulaChar.ToString();
                i++;
            }
            // 指定位置の文字が数字の場合
            else if (char.IsDigit(formulaChar))
            {
                int start = i;
                bool dotContains = false;

                while (true)
                {
                    i++;

                    if (i >= formula.Length)
                    {
                        break;
                    }

                    formulaChar = formula[i];

                    if (formulaChar == '.')
                    {
                        if (dotContains)
                        {
                            throw new FormatException();
                        }
                        else
                        {
                            dotContains = true;
                            continue;
                        }
                    }

                    if (char.IsLetter(formulaChar))
                    {
                        throw new FormatException();
                    }
                    else if (!char.IsDigit(formulaChar))
                    {
                        break;
                    }
                }

                yield return formula[start..i];
            }
            // 指定位置の文字がアルファベットの場合
            else if (char.IsLetter(formulaChar))
            {
                int start = i;

                do
                {
                    i++;
                }
                while (i < formula.Length && char.IsLetterOrDigit(formula[i]));

                yield return formula[start..i];
            }
            // 上記以外の場合
            else
            {
                throw new FormatException();
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
        if (char.IsLetterOrDigit(token[0]))
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
