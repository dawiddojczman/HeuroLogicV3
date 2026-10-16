using HeuroLogicV3.Core.Models;
using HeuroLogicV3.Core.Services;
using NUnit.Framework;

namespace HeuroLogicV3.Tests.UnitTests;

[TestFixture]
public class AhpServiceTests
{
    private readonly AhpService _service = new();

    [Test]
    public void AnalyzeMatrix_ConsistentThreeByThree_ReturnsPrincipalEigenvector()
    {
        var matrix = new double[,]
        {
            { 1, 2, 4 },
            { 1d / 2, 1, 2 },
            { 1d / 4, 1d / 2, 1 }
        };

        var result = _service.AnalyzeMatrix(matrix);

        Assert.Multiple(() =>
        {
            Assert.That(result.Weights[0], Is.EqualTo(4d / 7).Within(1e-10));
            Assert.That(result.Weights[1], Is.EqualTo(2d / 7).Within(1e-10));
            Assert.That(result.Weights[2], Is.EqualTo(1d / 7).Within(1e-10));
            Assert.That(result.Weights.Sum(), Is.EqualTo(1).Within(1e-10));
            Assert.That(result.LambdaMax, Is.EqualTo(3).Within(1e-10));
            Assert.That(result.ConsistencyIndex, Is.EqualTo(0).Within(1e-10));
            Assert.That(result.ConsistencyRatio, Is.EqualTo(0).Within(1e-10));
            Assert.That(result.IsConsistent, Is.True);
        });
    }

    [Test]
    public void AnalyzeMatrix_InconsistentJudgments_HasCrAboveThreshold()
    {
        var matrix = new double[,]
        {
            { 1, 9, 1d / 9 },
            { 1d / 9, 1, 9 },
            { 9, 1d / 9, 1 }
        };

        var result = _service.AnalyzeMatrix(matrix);

        Assert.That(result.IsConsistent, Is.False);
        Assert.That(result.ConsistencyRatio, Is.GreaterThan(0.10));
    }

    [Test]
    public void AnalyzeMatrix_TwoByTwo_HasZeroConsistencyRatio()
    {
        var result = _service.AnalyzeMatrix(
            new double[,]
            {
                { 1, 9 },
                { 1d / 9, 1 }
            });

        Assert.That(result.Weights[0], Is.EqualTo(0.9).Within(1e-10));
        Assert.That(result.Weights[1], Is.EqualTo(0.1).Within(1e-10));
        Assert.That(result.ConsistencyRatio, Is.Zero);
    }

    [Test]
    public void Calculate_TwoCriteriaTwoOptions_ReturnsCorrectWeightedRanking()
    {
        var price = new Criteria
        {
            Name = "Price"
        };

        var quality = new Criteria
        {
            Name = "Quality"
        };

        var a = new Option
        {
            Name = "A"
        };

        var b = new Option
        {
            Name = "B"
        };

        var decision = new DecisionMatrix
        {
            Criterias = new List<Criteria>
            {
                price,
                quality
            },
            Options = new List<Option>
            {
                a,
                b
            },
            CriteriaPairwiseMatrix = new double[,]
            {
                { 1, 9 },
                { 1d / 9, 1 }
            },
            OptionPairwiseMatrices =
                new Dictionary<Guid, double[,]>
                {
                    [price.Id] = new double[,]
                    {
                        { 1, 1d / 3 },
                        { 3, 1 }
                    },

                    [quality.Id] = new double[,]
                    {
                        { 1, 5 },
                        { 1d / 5, 1 }
                    }
                }
        };

        var result = _service.Calculate(decision);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsConsistent, Is.True);
            Assert.That(result.RankedOptions[0].OptionId, Is.EqualTo(b.Id));
            Assert.That(result.RankedOptions[0].Score, Is.EqualTo(0.6916666666666667).Within(1e-10));
            Assert.That(result.RankedOptions[1].Score, Is.EqualTo(0.3083333333333333).Within(1e-10));
            Assert.That(result.RankedOptions.Sum(r => r.Score), Is.EqualTo(1).Within(1e-10));
        });
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(10)]
    public void AnalyzeMatrix_InvalidJudgment_Throws(int invalid)
    {
        var matrix = new double[,]
        {
            { 1, invalid },
            { 1, 1 }
        };

        Assert.Throws<ArgumentException>(() => _service.AnalyzeMatrix(matrix));
    }

    [Test]
    public void AnalyzeMatrix_NonReciprocalMatrix_Throws()
    {
        var matrix = new double[,]
        {
            { 1, 3 },
            { 0.5, 1 }
        };

        Assert.Throws<ArgumentException>(() => _service.AnalyzeMatrix(matrix));
    }

    [Test]
    public void AnalyzeMatrix_NaNMatrix_Throws()
    {
        var matrix = new double[,]
        {
            { 1, double.NaN },
            { 1, 1 }
        };

        Assert.Throws<ArgumentException>(() => _service.AnalyzeMatrix(matrix));
    }

    [Test]
    public void Calculate_EmptyDecision_Throws()
    {
        Assert.Throws<ArgumentException>(() => _service.Calculate(new DecisionMatrix()));
    }
}