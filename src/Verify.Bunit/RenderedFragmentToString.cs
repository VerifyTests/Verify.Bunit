static class RenderedFragmentToString
{
    public static ConversionResult Convert(object fragment, IReadOnlyDictionary<string, object> context)
    {
        dynamic dynamicFragment = fragment;
        var nodes = (IMarkupFormattable)(INodeList)dynamicFragment.Nodes;
        var markup = nodes.ToHtml()?.Trim() ?? string.Empty;
        var nodeCount = ((INodeList)dynamicFragment.Nodes).Sum(_ => _
            .GetDescendantsAndSelf()
            .Count());
        var info = new FragmentInfo(ComponentReader.GetInstance(fragment), nodeCount);
        var targets = new[] { new Target("html", markup) };
        return new(info, targets);
    }
}