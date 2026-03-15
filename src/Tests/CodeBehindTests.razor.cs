namespace testing;

public class CodeBehindTests
{
    [Fact]
    public Task Component()
    {
        using var context = new BunitContext();
        var component = context.Render<BlazorApp.TestComponent>(
            builder =>
            {
                builder.Add(_ => _.Title, "Code Behind Title");
                builder.Add(_ => _.Person, new() { Name = "Sam" });
            });
        return Verify(component);
    }
}
