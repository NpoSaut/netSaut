using NUnit.Framework;
using Saut.StateModel.Interpolators.InterpolationTools;

namespace Saut.StateModel.Test.Interpolators.InterpolationTools
{
    [TestFixture]
    public class NumericWeightingToolTests
    {
        [Test, Description("Проверяет правильность вычисления взвешенного арифметического среднего")]
        public void WeightedArithmeticMeanTest()
        {
            var weightingTool = new NumericWeightingTool();
            Assert.AreEqual(15, weightingTool.GetWeightedArithmeticMean(10, 20, 0.5), "Не верное значение среднего в середине отрезка");
            Assert.AreEqual(10, weightingTool.GetWeightedArithmeticMean(10, 20, 0.0), "Не верное значение среднего на левом конце отрезка");
            Assert.AreEqual(20, weightingTool.GetWeightedArithmeticMean(10, 20, 1.0), "Не верное значение среднего на правом конце отрезка");
            Assert.AreEqual(13, weightingTool.GetWeightedArithmeticMean(10, 20, 0.3), "Не верное значение среднего внутри отрезка");
        }
    }
}