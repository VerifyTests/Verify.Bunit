static class RenderedFragmentMarkupToString
{
    public static ConversionResult Convert(IRenderedComponent<IComponent> fragment, IReadOnlyDictionary<string, object> context)
    {
        var formatter = new PrettyMarkupFormatter
        {
            Indentation = "  "
        };
        var markup = fragment
            .Nodes
            .ToHtml(formatter)
            .Trim();
        return new(null, "html", markup);
    }
}