using AngleSharp.Diffing.Core;
using AngleSharp.Text;
using CompareResult = VerifyTests.CompareResult;

static class BunitMarkupComparer
{
    // AngleSharp.Diffing compares style attributes semantically, which needs the parsed markup to carry
    // a real CSS object model. A default HtmlParser has no CSS support, so an element's inline style
    // parses to null and StyleAttributeComparer throws a NullReferenceException the moment two style
    // attributes differ. Parsing through a CSS-enabled configuration gives every element a real
    // ICssStyleDeclaration, so the style comparison works.
    static readonly IConfiguration configuration = Configuration.Default.WithCss();

    public static Task<CompareResult> Compare(string received, string verified, IReadOnlyDictionary<string, object> context)
    {
        var parser = new HtmlParser(new(), BrowsingContext.New(configuration));
        var receivedDoc = parser.ParseDocument(received);
        var verifiedDoc = parser.ParseDocument(verified);
        // Compare the whole document element rather than just the body. This is the global "html"
        // comparer, so it also receives complete documents, not only bUnit fragments; comparing only
        // the body let a difference confined to the head — title, meta, a stylesheet — slip through as
        // equal. A parsed fragment has an empty head on both sides, so this is a no-op for components.
        var diffs = receivedDoc.DocumentElement!.ChildNodes.CompareTo(verifiedDoc.DocumentElement!.ChildNodes);

        var result = diffs.Count == 0
            ? CompareResult.Equal
            : CompareResult.NotEqual(CreateDiffMessage(received, verified, diffs));

        return Task.FromResult(result);
    }

    static string CreateDiffMessage(string received, string verified, IReadOnlyList<IDiff> diffs)
    {
        var builder = StringBuilderPool.Obtain();
        builder.AppendLine();
        builder.AppendLine("HTML comparison failed. The following errors were found:");

        for (var i = 0; i < diffs.Count; i++)
        {
            builder.Append($"  {i + 1}: ");
            builder.AppendLine(diffs[i] switch
            {
                NodeDiff {Target: DiffTarget.Text} diff when diff.Control.Path.Equals(diff.Test.Path, StringComparison.Ordinal)
                    => $"The text in {diff.Control.Path} is different.",
                NodeDiff {Target: DiffTarget.Text} diff => $"The expected {NodeName(diff.Control)} at {diff.Control.Path} and the actual {NodeName(diff.Test)} at {diff.Test.Path} is different.",
                NodeDiff diff when diff.Control.Path.Equals(diff.Test.Path, StringComparison.Ordinal)
                    => $"The {NodeName(diff.Control)}s at {diff.Control.Path} are different.",
                NodeDiff diff => $"The expected {NodeName(diff.Control)} at {diff.Control.Path} and the actual {NodeName(diff.Test)} at {diff.Test.Path} are different.",
                AttrDiff diff when diff.Control.Path.Equals(diff.Test.Path, StringComparison.Ordinal)
                    => $"The values of the attributes at {diff.Control.Path} are different.",
                AttrDiff diff => $"The value of the attribute {diff.Control.Path} and actual attribute {diff.Test.Path} are different.",
                MissingNodeDiff diff => $"The {NodeName(diff.Control)} at {diff.Control.Path} is missing.",
                MissingAttrDiff diff => $"The attribute at {diff.Control.Path} is missing.",
                UnexpectedNodeDiff diff => $"The {NodeName(diff.Test)} at {diff.Test.Path} was not expected.",
                UnexpectedAttrDiff diff => $"The attribute at {diff.Test.Path} was not expected.",
                _ => throw new SwitchExpressionException($"Unknown diff type detected: {diffs[i].GetType()}")
            });
        }

        builder.AppendLine(
            $"""

             Actual HTML:

             {received}

             Expected HTML:

             {verified}
             """);

        return builder.ToPool();

        static string NodeName(ComparisonSource source) =>
            source
                .Node.NodeType.ToString()
                .ToLowerInvariant();
    }
}