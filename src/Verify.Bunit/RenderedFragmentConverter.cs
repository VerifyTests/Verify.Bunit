class RenderedFragmentConverter :
    WriteOnlyJsonConverter<IRenderedComponent<IComponent>>
{
    public override void Write(VerifyJsonWriter writer, IRenderedComponent<IComponent> component)
    {
        writer.WriteStartObject();

        var instance = ComponentReader.GetInstance(component);
        if (instance != null)
        {
            writer.WriteMember(component, instance, PrettyName(instance.GetType()));
        }

        writer.WriteMember(
            component,
            component.Markup.Trim(),
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