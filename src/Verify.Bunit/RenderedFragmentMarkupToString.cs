static class RenderedFragmentMarkupToString
{
    public static ConversionResult Convert(IRenderedComponent<IComponent> component, IReadOnlyDictionary<string, object> context)
    {
        var markup = component.Markup.Trim();
        return new(null, "html", markup);
    }
}