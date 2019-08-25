using Calculator.Common.Function;
using System;
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

        [Fact]
        public void ConvertNormalFormula1()
        {
            var result = ReversePolishNotation.Convert("11+234");
            Assert.Equal(new[] { "11", "234", "+" }, result);
        }

        [Fact]
        public void ConvertNormalFormula2()
        {
            var result = ReversePolishNotation.Convert("11+234*ab56");
            Assert.Equal(new[] { "11", "234", "ab56", "*", "+" }, result);
        }

        [Fact]
        public void ConvertNormalFormula3()
        {
            var result = ReversePolishNotation.Convert("11--234*ab56");
            Assert.Equal(new[] { "11", "234", "-@", "ab56", "*", "-" }, result);
        }

        [Fact]
        public void ConvertNormalFormula4()
        {
            var result = ReversePolishNotation.Convert("11*(234-ab56)*7");
            Assert.Equal(new[] { "11", "234", "ab56", "-", "*", "7", "*" }, result);
        }

        [Fact]
        public void ConvertNormalFormula5()
        {
            var result = ReversePolishNotation.Convert("11*((234-ab56)*7)");
            Assert.Equal(new[] { "11", "234", "ab56", "-", "7", "*", "*" }, result);
        }

        [Fact]
        public void ConvertInvalidFormula1()
        {
            var result = ReversePolishNotation.Convert("11+234a56");
            Assert.Throws<FormatException>(() => result.ToList());
        }

        [Fact]
        public void ConvertInvalidFormula2()
        {
            var result = ReversePolishNotation.Convert("11+++234");
            Assert.Throws<FormatException>(() => result.ToList());
        }

        [Fact]
        public void ConvertInvalidFormula3()
        {
            var result = ReversePolishNotation.Convert("11*(234-ab56*7");
            Assert.Throws<FormatException>(() => result.ToList());
        }

        [Fact]
        public void ConvertInvalidFormula4()
        {
            var result = ReversePolishNotation.Convert("11*((234-ab56)*7))");
            Assert.Throws<FormatException>(() => result.ToList());
        }

        [Fact]
        public void ConvertInvalidFormula5()
        {
            var result = ReversePolishNotation.Convert("11+234 56");
            Assert.Throws<FormatException>(() => result.ToList());
        }
    }
}
