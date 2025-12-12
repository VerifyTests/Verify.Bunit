static class RenderedFragmentToString
{
    public static ConversionResult Convert(object fragment, IReadOnlyDictionary<string, object> context)
    {
        dynamic dynamicFragment = fragment;
        var nodes = GetMarkupFormattable(dynamicFragment.Nodes);
        var markup = nodes.ToHtml()?.Trim() ?? string.Empty;
        var nodeCount = ((INodeList)dynamicFragment.Nodes).Sum(_ => _
            .GetDescendantsAndSelf()
            .Count());
        var info = new FragmentInfo(ComponentReader.GetInstance(fragment), nodeCount);
        var targets = new[] { new Target("html", markup) };
        return new(info, targets);
    }

    static IMarkupFormattable GetMarkupFormattable(dynamic nodes) =>
        (IMarkupFormattable)(INodeList)nodes;
}