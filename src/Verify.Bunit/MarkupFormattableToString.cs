using AngleSharp.Html;

static class MarkupFormattableToString
{
    public static ConversionResult Convert(IMarkupFormattable markup, IReadOnlyDictionary<string, object> context)
    {
        using var sw = new StringWriter();
        markup.ToHtml(sw, HtmlMarkupFormatter.Instance);
        return new(null, "html", sw.ToString().Trim());
    }
}