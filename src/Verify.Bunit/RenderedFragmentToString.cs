static class RenderedFragmentToString
{
    public static ConversionResult Convert(IRenderedComponent<IComponent> component, IReadOnlyDictionary<string, object> context)
    {
        var nodes = component.Nodes;
        var markup = component.Markup.Trim();
        var nodeCount = nodes.Sum(_ => _
            .GetDescendantsAndSelf()
            .Count());
        var info = new FragmentInfo(ComponentReader.GetInstance(component), nodeCount);
        return new(info, "html", markup);
    }
}