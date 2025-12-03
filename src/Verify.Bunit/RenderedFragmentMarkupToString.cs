static class RenderedFragmentMarkupToString
{
    public static ConversionResult Convert(object fragment, IReadOnlyDictionary<string, object> context)
    {
        dynamic dynamicFragment = fragment;
        var nodes = (INodeList)dynamicFragment.Nodes;
        var markup = nodes.ToDiffMarkup().Trim();
        return new(null, "html", markup);
    }
}