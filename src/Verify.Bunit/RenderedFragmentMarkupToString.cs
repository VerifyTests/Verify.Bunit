static class RenderedFragmentMarkupToString
{
    public static ConversionResult Convert(IRenderedComponent<IComponent> fragment, IReadOnlyDictionary<string, object> context)
    {
        var markup = fragment
            .Nodes.ToHtml(new PrettyMarkupFormatter { Indentation = "  " })
            .Trim();
        return new(null, "html", markup);
    }
}