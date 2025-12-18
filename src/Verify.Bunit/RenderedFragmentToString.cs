static class RenderedFragmentToString
{
    public static ConversionResult Convert(IRenderedComponent<IComponent> fragment, IReadOnlyDictionary<string, object> context)
    {
        var nodes = fragment.Nodes;
        var markup = nodes
            .ToHtml(new PrettyMarkupFormatter { Indentation = "  " })
            .Trim();
        var nodeCount = nodes.Sum(_ => _
            .GetDescendantsAndSelf()
            .Count());
        var info = new FragmentInfo(fragment.Instance, nodeCount);
        return new(info, "html", markup);
    }
}