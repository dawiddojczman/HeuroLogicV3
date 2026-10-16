using HeuroLogicV3.Core.Models;

namespace HeuroLogicV3.Core.Services;

public sealed class AhpService
{
    private static readonly double[] RandomIndex =
    {
        0, 0, 0, 0.58, 0.90,
        1.12, 1.24, 1.32, 1.41,
        1.45, 1.49
    };

    public AhpDecisionResult Calculate(DecisionMatrix decision)
    {
        ArgumentNullException.ThrowIfNull(decision);

        ValidateDecision(decision);

        var criteriaResult = AnalyzeMatrix(decision.CriteriaPairwiseMatrix);
        var alternatives = new Dictionary<Guid, AhpMatrixResult>();

        foreach (var criterion in decision.Criterias)
        {
            if (!decision.OptionPairwiseMatrices.TryGetValue(criterion.Id, out var matrix))
            {
                throw new ArgumentException($"Missing option comparison matrix for '{criterion.Name}'.");
            }

            alternatives.Add(criterion.Id, AnalyzeMatrix(matrix));
        }

        var ranking = decision.Options
            .Select((option, optionIndex) =>
            {
                var score = decision.Criterias
                    .Select((criterion, criterionIndex) => criteriaResult.Weights[criterionIndex] * alternatives[criterion.Id].Weights[optionIndex])
                    .Sum();

                return new RankedOption(option.Id, option.Name, score);
            })
            .OrderByDescending(option => option.Score)
            .ThenBy(option => option.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return new AhpDecisionResult
        {
            CriteriaAnalysis = criteriaResult,
            AlternativeAnalyses = alternatives,
            RankedOptions = ranking
        };
    }

    public AhpMatrixResult AnalyzeMatrix(double[,] matrix)
    {
        ArgumentNullException.ThrowIfNull(matrix);

        ValidateMatrix(matrix);

        int size = matrix.GetLength(0);

        var weights = Enumerable
            .Repeat(1d / size, size)
            .ToArray();

        var next = new double[size];

        bool converged = false;

        for (int iteration = 0; iteration < 10_000; iteration++)
        {
            for (int row = 0; row < size; row++)
            {
                next[row] = 0;

                for (int col = 0; col < size; col++)
                {
                    next[row] += matrix[row, col] * weights[col];
                }
            }

            double total = next.Sum();

            for (int row = 0; row < size; row++)
            {
                next[row] /= total;
            }

            double maxDifference = weights
                .Zip(next, (a, b) => Math.Abs(a - b))
                .Max();

            Array.Copy(next, weights, size);

            if (maxDifference < 1e-13)
            {
                converged = true;
                break;
            }
        }

        if (!converged)
        {
            throw new InvalidOperationException("AHP eigenvector calculation did not converge.");
        }

        double lambdaMax = 0;

        for (int row = 0; row < size; row++)
        {
            double weightedSum = 0;

            for (int col = 0; col < size; col++)
            {
                weightedSum += matrix[row, col] * weights[col];
            }

            lambdaMax += weightedSum / weights[row];
        }

        lambdaMax /= size;

        double ci = size <= 2 ? 0 : Math.Max(0, (lambdaMax - size) / (size - 1));
        double cr = size <= 2 ? 0 : ci / RandomIndex[size];

        return new AhpMatrixResult(Array.AsReadOnly(weights), lambdaMax, ci, cr);
    }

    private static void ValidateDecision(DecisionMatrix decision)
    {
        if (decision.Criterias is null || decision.Criterias.Count is < 2 or > 10)
        {
            throw new ArgumentException("Provide between 2 and 10 criteria.");
        }

        if (decision.Options is null || decision.Options.Count is < 2 or > 10)
        {
            throw new ArgumentException("Provide between 2 and 10 options.");
        }

        if (decision.Criterias.Any(c => string.IsNullOrWhiteSpace(c.Name)) || decision.Options.Any(o => string.IsNullOrWhiteSpace(o.Name)))
        {
            throw new ArgumentException("Criteria and option names cannot be blank.");
        }

        if (decision.Criterias
                .Select(c => c.Id)
                .Distinct()
                .Count() != decision.Criterias.Count ||

            decision.Options
                .Select(o => o.Id)
                .Distinct()
                .Count() != decision.Options.Count ||

            decision.Criterias
                .Select(c => c.Name.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count() != decision.Criterias.Count ||

            decision.Options
                .Select(o => o.Name.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count() != decision.Options.Count)
        {
            throw new ArgumentException("Criteria and option identifiers/names must be unique.");
        }

        if (decision.CriteriaPairwiseMatrix.GetLength(0) != decision.Criterias.Count || decision.CriteriaPairwiseMatrix.GetLength(1) != decision.Criterias.Count)
        {
            throw new ArgumentException("Criteria matrix dimensions do not match criteria count.");
        }

        if (decision.OptionPairwiseMatrices is null || decision.OptionPairwiseMatrices.Count != decision.Criterias.Count)
        {
            throw new ArgumentException("Exactly one option matrix is required per criterion.");
        }

        foreach (var criterion in decision.Criterias)
        {
            if (!decision.OptionPairwiseMatrices.TryGetValue(criterion.Id, out var matrix) || matrix is null || matrix.GetLength(0) != decision.Options.Count || matrix.GetLength(1) != decision.Options.Count)
            {
                throw new ArgumentException($"Invalid option matrix for criterion '{criterion.Name}'.");
            }
        }
    }

    private static void ValidateMatrix(double[,] matrix)
    {
        int n = matrix.GetLength(0);

        if (n < 2 || n > 10 || matrix.GetLength(1) != n)
        {
            throw new ArgumentException("AHP matrix must be square with size 2 to 10.", nameof(matrix));
        }

        for (int i = 0; i < n; i++)
        {
            if (!double.IsFinite(matrix[i, i]) || Math.Abs(matrix[i, i] - 1d) > 1e-9)
            {
                throw new ArgumentException("Diagonal matrix elements must equal 1.", nameof(matrix));
            }

            for (int j = i + 1; j < n; j++)
            {
                double forward = matrix[i, j];
                double backward = matrix[j, i];

                if (!double.IsFinite(forward) || !double.IsFinite(backward) ||
                    forward < 1d / 9d - 1e-12 ||
                    forward > 9 + 1e-12 ||
                    backward < 1d / 9d - 1e-12 ||
                    backward > 9 + 1e-12 ||
                    Math.Abs(forward * backward - 1) > 1e-8)
                {
                    throw new ArgumentException("Matrix judgments must be finite, reciprocal and between 1/9 and 9.", nameof(matrix));
                }
            }
        }
    }
}