using ClearHaul.Domain.Records;

namespace ClearHaul.Domain.Rules;

public sealed record RegulatorySource(string Id, string Publisher, string Title, string RetrievedNote);

public sealed record RegulatoryCitation(string Id, string SourceId, string Locator, DateOnly Effective, DateOnly? Retired);

public sealed record RuleDefinition(
    string Id,
    string Question,
    string FactName,
    string Operator,
    string Operand,
    string DecisionCode,
    string CitationId);

public sealed record RulePackage(
    string Id,
    string Version,
    DateOnly Effective,
    DateOnly? Retired,
    bool ApprovedForLive,
    bool SyntheticTestFixture,
    IReadOnlyList<RuleDefinition> Rules);

public sealed record ComplianceDecision(
    string Outcome,
    string DecisionCode,
    IReadOnlyList<string> InputFacts,
    string Jurisdiction,
    string Citation,
    string RuleVersion,
    string EffectiveDate,
    string Explanation,
    string WarningLevel,
    bool HumanConfirmationRequired,
    bool BlocksDispatch)
{
    public const string UnableOutcome = "UNABLE TO DETERMINE";

    public const string QualifiedReviewCode = "QUALIFIED HAZMAT REVIEW REQUIRED";

    public static ComplianceDecision Unable(string explanation, IReadOnlyList<string> facts, bool blocks) =>
        new(
            UnableOutcome,
            QualifiedReviewCode,
            facts,
            "unspecified",
            "no-approved-source",
            "none",
            "none",
            explanation,
            "high",
            true,
            blocks);

    public static ComplianceDecision NotApplicable(string explanation) =>
        new(
            "NOT_APPLICABLE",
            "not-applicable",
            [],
            "unspecified",
            "none",
            "none",
            "none",
            explanation,
            "none",
            false,
            false);
}

public sealed class RulesEngine
{
    private readonly List<RulePackage> _packages = [];

    public IReadOnlyList<RulePackage> Packages => _packages;

    public OperationResult AddDraft(RulePackage package)
    {
        if (package.ApprovedForLive)
        {
            return OperationResult.Fail(
                "live-approval-refused",
                "No role can mark a rule package approved for live use.");
        }

        _packages.Add(package);
        return OperationResult.Ok("rule-package-drafted", package.Id);
    }

    public ComplianceDecision Evaluate(string? packageId, string question, IReadOnlyDictionary<string, string> facts, bool live)
    {
        var package = _packages.FirstOrDefault(item => item.Id == packageId);
        if (package is null || (live && (package.SyntheticTestFixture || !package.ApprovedForLive)))
        {
            var inputs = facts.Select(pair => pair.Key + "=" + pair.Value).ToArray();
            return ComplianceDecision.Unable(
                "No approved rule package is deployed. This software did not invent a regulatory answer.",
                inputs,
                blocks: true);
        }

        var matched = package.Rules.FirstOrDefault(rule =>
            rule.Question == question
            && facts.TryGetValue(rule.FactName, out var actual)
            && Compare(rule.Operator, actual, rule.Operand));
        if (matched is null)
        {
            return ComplianceDecision.Unable(
                "The bound package has no matching rule for these facts.",
                facts.Select(pair => pair.Key + "=" + pair.Value).ToArray(),
                blocks: true);
        }

        var citation = matched.CitationId;
        return new ComplianceDecision(
            "DETERMINED_BY_FIXTURE",
            matched.DecisionCode,
            facts.Select(pair => pair.Key + "=" + pair.Value).ToArray(),
            "test-fixture",
            citation,
            package.Version,
            package.Effective.ToString("yyyy-MM-dd"),
            "This result comes from a synthetic test fixture, not from a reviewed regulation.",
            "review",
            true,
            matched.DecisionCode.Contains("block", StringComparison.OrdinalIgnoreCase));
    }

    private static bool Compare(string op, string actual, string operand) =>
        op switch
        {
            "eq" => string.Equals(actual, operand, StringComparison.Ordinal),
            "gt" => long.TryParse(actual, out var left) && long.TryParse(operand, out var right) && left > right,
            _ => false
        };
}
