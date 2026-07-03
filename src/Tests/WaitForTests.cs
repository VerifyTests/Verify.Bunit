public class WaitForTests
{
    // Regression: Inner derived its retry count from TimeSpan.Milliseconds (the 0-999 component) rather
    // than TotalMilliseconds, so a timeout with a whole-second part — the common case — collapsed to zero
    // retries and threw TimeoutException on the very first poll. The predicate below only turns true on
    // its third evaluation, so under a one-second timeout the wait must retry rather than give up at once;
    // with the bug this throws instead of returning the component.
    [Fact]
    public async Task RenderComponentAndWait_retries_under_a_whole_second_timeout()
    {
        using var context = new BunitContext();
        var polls = 0;

        var component = await context.RenderComponentAndWait<TestComponent>(
            builder =>
            {
                builder.Add(_ => _.Title, "New Title");
                builder.Add(_ => _.Person, new() { Name = "Sam" });
            },
            _ => ++polls >= 3,
            TimeSpan.FromSeconds(1));

        Assert.NotNull(component);
        Assert.True(polls >= 3);
    }
}
