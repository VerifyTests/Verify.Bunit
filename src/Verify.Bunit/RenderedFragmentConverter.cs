class RenderedFragmentConverter :
    WriteOnlyJsonConverter
{
    public override void Write(VerifyJsonWriter writer, object value)
    {
        // Cast to dynamic to access properties at runtime
        dynamic fragment = value;
        
        writer.WriteStartObject();

        var instance = ComponentReader.GetInstance(value);
        if (instance != null)
        {
            writer.WriteMember(value, instance, PrettyName(instance.GetType()));
        }

        writer.WriteMember(
            value,
            ((IMarkupFormattable)(INodeList)fragment.Nodes)
            .ToHtml()
            .Trim(),
            "Markup");
        writer.WriteEndObject();
    }

    public override bool CanConvert(Type type) =>
        type.IsGenericType &&
        type.GetGenericTypeDefinition().FullName == "Bunit.IRenderedComponent`1";

    static string PrettyName(Type type)
    {
        var genericArguments = type.GetGenericArguments();
        if (genericArguments.Length == 0)
        {
            return type.Name;
        }

        var typeName = type.Name;
        var unmangledName = typeName[..typeName.IndexOf('`')];
        return $"{unmangledName}<{string.Join(',', genericArguments.Select(PrettyName))}>";
    }
}