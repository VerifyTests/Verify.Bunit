using Microsoft.AspNetCore.Components;

static class ComponentReader
{
    public static IComponent? GetInstance(object fragment)
    {
        var type = fragment.GetType();
        if (!type.IsGenericType)
        {
            return null;
        }

        var componentInterface = type
            .GetInterfaces()
            .SingleOrDefault(_ =>
                _.IsGenericType &&
                _.GetGenericTypeDefinition().FullName == "Bunit.IRenderedComponent`1");

        if (componentInterface == null)
        {
            return null;
        }

        var instanceProperty = componentInterface.GetProperty("Instance")!;
        return (IComponent) instanceProperty.GetValue(fragment)!;
    }
}