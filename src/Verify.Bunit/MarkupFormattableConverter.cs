class MarkupFormattableConverter :
    WriteOnlyJsonConverter<IMarkupFormattable>
{
    public override void Write(VerifyJsonWriter writer, IMarkupFormattable markup)
    {
        writer.WriteStartObject();
        writer.WriteMember(
            markup,
            markup
            .ToHtml(DiffMarkupFormatter.Instance)
            .Trim(),
            "Markup");
        writer.WriteEndObject();
    }
}