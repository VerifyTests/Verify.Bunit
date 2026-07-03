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
        // A non-generic type nested in a generic one (e.g. Outer<T>.Inner) reports the enclosing type's
        // arguments but carries no backtick in its own name, so guard the mangling marker rather than
        // slicing at index -1.
        var backtick = typeName.IndexOf('`');
        if (backtick == -1)
        {
            return typeName;
        }

        var unmangledName = typeName[..backtick];
        return $"{unmangledName}<{string.Join(',', genericArguments.Select(PrettyName))}>";
    }
}