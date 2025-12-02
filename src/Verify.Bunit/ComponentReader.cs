static class ComponentReader
{
    public static IComponent? GetInstance<TComponent>(IRenderedComponent<TComponent> component) where TComponent : IComponent =>
        component.Instance;
}