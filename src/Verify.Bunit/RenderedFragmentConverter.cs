class RenderedFragmentConverter :
    WriteOnlyJsonConverter<IRenderedComponent<IComponent>>
{
    public override void Write(VerifyJsonWriter writer, IRenderedComponent<IComponent> fragment)
    {
        writer.WriteStartObject();

        var instance = fragment.Instance;
        writer.WriteMember(fragment, instance, PrettyName(instance.GetType()));

        writer.WriteMember(
            fragment,
            fragment
            .Nodes.ToHtml(new PrettyMarkupFormatter { Indentation = "  " })
            .Trim(),
            "Markup");
        writer.WriteEndObject();
    }

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