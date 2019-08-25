using Calculator.Common.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Calculator.Common.Tests.Function
{
    public class ReversePolishNotationTests
    {
        [Fact]
        public void ConvertNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => ReversePolishNotation.Convert(null));
            Assert.Equal("formula", ex.ParamName);
        }

        [Theory]
        [MemberData(nameof(ConvertNormalFormulaData))]
        public void ConvertNormalFormula(string formula, IEnumerable<string> expected)
        {
            var result = ReversePolishNotation.Convert(formula);
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> ConvertNormalFormulaData()
        {
            yield return new object[]
            {
                "11+234",
                new string[] { "11", "234", "+" }
            };

            yield return new object[]
            {
                "11+234*ab56",
                new string[] { "11", "234", "ab56", "*", "+" }
            };

            yield return new object[]
            {
                "11--234*ab56",
                new string[] { "11", "234", "-@", "ab56", "*", "-" }
            };

            yield return new object[]
            {
                "11*(234-ab56)*7",
                new string[] { "11", "234", "ab56", "-", "*", "7", "*" }
            };

            yield return new object[]
            {
                "11*((234-ab56)*7)",
                new string[] { "11", "234", "ab56", "-", "7", "*", "*" }
            };
        }

        [Theory]
        [MemberData(nameof(ConvertInvalidFormulaData))]
        public void ConvertInvalidFormula(string formula)
        {
            var result = ReversePolishNotation.Convert(formula);
            Assert.Throws<FormatException>(() => result.ToList());
        }

        public static IEnumerable<object[]> ConvertInvalidFormulaData()
        {
            yield return new object[] { "11+234a56" };
            yield return new object[] { "11+++234" };
            yield return new object[] { "11*(234-ab56*7" };
            yield return new object[] { "11*((234-ab56)*7))" };
            yield return new object[] { "11+234 56" };
        }
    }
}
