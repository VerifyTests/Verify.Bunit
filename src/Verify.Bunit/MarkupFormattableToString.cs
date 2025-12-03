static class MarkupFormattableToString
{
    public static ConversionResult Convert(IMarkupFormattable markup, IReadOnlyDictionary<string, object> context)
    {
        var html = markup.ToHtml()?.Trim() ?? string.Empty;
        var targets = new[] { new Target("html", html) };
        return new(null, targets);
    }
}