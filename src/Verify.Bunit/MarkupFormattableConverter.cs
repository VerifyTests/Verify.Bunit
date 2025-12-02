using AngleSharp.Html;

class MarkupFormattableConverter :
    WriteOnlyJsonConverter<IMarkupFormattable>
{
    public override void Write(VerifyJsonWriter writer, IMarkupFormattable markup)
    {
        writer.WriteStartObject();
        writer.WriteMember(
            markup,
            ToMarkup(markup),
            "Markup");
        writer.WriteEndObject();
    }

    static string ToMarkup(IMarkupFormattable markup)
    {
        using var sw = new StringWriter();
        markup.ToHtml(sw, HtmlMarkupFormatter.Instance);
        return sw.ToString().Trim();
    }
}