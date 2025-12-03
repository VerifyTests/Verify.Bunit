static class RenderedFragmentMarkupToString
{
    public static ConversionResult Convert(object fragment, IReadOnlyDictionary<string, object> context)
    {
        dynamic dynamicFragment = fragment;
        var nodes = GetMarkupFormattable(dynamicFragment.Nodes);
        var markup = nodes.ToHtml()?.Trim() ?? string.Empty;
        var targets = new[] { new Target("html", markup) };
        return new(null, targets);
    }

    static IMarkupFormattable GetMarkupFormattable(dynamic nodes) =>
        (IMarkupFormattable)(INodeList)nodes;
}