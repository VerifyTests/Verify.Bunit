static class RenderedFragmentMarkupToString
{
    public static ConversionResult Convert(IRenderedFragment fragment, IReadOnlyDictionary<string, object> context)
    {
        var markup = fragment
            .Nodes.ToHtml(DiffMarkupFormatter.Instance)
            .Trim();
        return new(null, "html", markup);
    }
}