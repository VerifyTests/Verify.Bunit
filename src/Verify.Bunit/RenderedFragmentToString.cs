static class RenderedFragmentToString
{
    public static ConversionResult Convert(object fragment, IReadOnlyDictionary<string, object> context)
    {
        dynamic dynamicFragment = fragment;
        var nodes = (INodeList)dynamicFragment.Nodes;
        var markup = nodes.ToDiffMarkup().Trim();
        var nodeCount = nodes.Sum(_ => _
            .GetDescendantsAndSelf()
            .Count());
        var info = new FragmentInfo(ComponentReader.GetInstance(fragment), nodeCount);
        return new(info, "html", markup);
    }
}