namespace VerifyTests;

public static class VerifyBunit
{
    public static bool Initialized { get; private set; }

    public static void Initialize(bool excludeComponent = false)
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;

        InnerVerifier.ThrowIfVerifyHasBeenRun();

        if (excludeComponent)
        {
            VerifierSettings.RegisterFileConverter<IRenderedComponent<IComponent>>(RenderedFragmentMarkupToString.Convert);
        }
        else
        {
            VerifierSettings.RegisterFileConverter<IRenderedComponent<IComponent>>(RenderedFragmentToString.Convert);
        }

        VerifierSettings.RegisterFileConverter<IMarkupFormattable>(MarkupFormattableToString.Convert);

        VerifierSettings.AddExtraSettings(_ => _.Converters.Add(new RenderedFragmentConverter()));
        VerifierSettings.AddExtraSettings(_ => _.Converters.Add(new MarkupFormattableConverter()));
        VerifierSettings.RegisterStringComparer("html", BunitMarkupComparer.Compare);
    }

    /// <summary>
    /// Instantiates and performs a first render of a component of type <typeparamref name="TComponent" />.
    /// </summary>
    /// <typeparam name="TComponent">Type of the component to render.</typeparam>
    /// <param name="context">The <see cref="BunitContext" /> to extend.</param>
    /// <param name="parameterBuilder">The ComponentParameterBuilder action to add type safe parameters to pass to the component when it is rendered.</param>
    /// <param name="timeout">A TimeSpan that represents the to wait, or null to use 10 seconds.</param>
    /// <param name="renderedCheck">Checks if rendered has finished.</param>
    /// <returns>The rendered <typeparamref name="TComponent" />.</returns>
    public static Task<IRenderedComponent<TComponent>> RenderComponentAndWait<TComponent>(
        this BunitContext context,
        Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder,
        Func<TComponent, bool> renderedCheck,
        TimeSpan? timeout = null)
        where TComponent : IComponent =>
        Inner(() => context.Render(parameterBuilder), timeout, renderedCheck);

    /// <summary>
    /// Renders the <paramref name="fragment" /> and returns the first <typeparamref name="TComponent" /> in the resulting render tree.
    /// </summary>
    /// <remarks>
    /// Calling this method is equivalent to calling <c>Render(renderFragment).FindComponent&lt;TComponent&gt;()</c>.
    /// </remarks>
    /// <typeparam name="TComponent">The type of component to find in the render tree.</typeparam>
    /// <param name="fragment">The render fragment to render.</param>
    /// <param name="context">The <see cref="BunitContext" /> to extend.</param>
    /// <param name="renderedCheck">Checks if rendered has finished.</param>
    /// <param name="timeout">A TimeSpan that represents the to wait, or null to use 10 seconds.</param>
    /// <returns>The <see cref="IRenderedComponent{TComponent}" />.</returns>
    public static Task<IRenderedComponent<TComponent>> RenderAndWait<TComponent>(
        this BunitContext context,
        RenderFragment fragment,
        Func<TComponent, bool> renderedCheck,
        TimeSpan? timeout = null)
        where TComponent : IComponent =>
        Inner(() => context.Render<TComponent>(fragment), timeout, renderedCheck);

    static async Task<IRenderedComponent<TComponent>> Inner<TComponent>(
        Func<IRenderedComponent<TComponent>> render,
        TimeSpan? timeout,
        Func<TComponent, bool> renderedCheck)
        where TComponent : IComponent
    {
        var iterations = timeout?.Milliseconds / 10 ?? 100;
        var target = render();

        var instance = target.Instance;
        while (!renderedCheck(instance))
        {
            if (iterations-- == 0)
            {
                throw new TimeoutException("Timeout waiting for component to render");
            }

            await Task.Delay(10);
        }

        return target;
    }
}