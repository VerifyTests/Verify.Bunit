public class BunitMarkupComparerTests
{
    static readonly IReadOnlyDictionary<string, object> context = new Dictionary<string, object>();

    // The comparer used to diff only <body>, so a difference confined to the <head> — a title, meta,
    // or stylesheet — slipped through as equal. It now compares the whole document element.
    [Fact]
    public async Task Detects_a_difference_in_the_head()
    {
        var received = "<html><head><title>Received</title></head><body><p>same</p></body></html>";
        var verified = "<html><head><title>Verified</title></head><body><p>same</p></body></html>";

        var result = await BunitMarkupComparer.Compare(received, verified, context);

        Assert.False(result.IsEqual);
    }

    [Fact]
    public async Task Detects_a_difference_in_the_body()
    {
        var received = "<html><head><title>Same</title></head><body><p>received</p></body></html>";
        var verified = "<html><head><title>Same</title></head><body><p>verified</p></body></html>";

        var result = await BunitMarkupComparer.Compare(received, verified, context);

        Assert.False(result.IsEqual);
    }

    [Fact]
    public async Task Treats_identical_documents_as_equal()
    {
        var html = "<html><head><title>Same</title></head><body><p>same</p></body></html>";

        var result = await BunitMarkupComparer.Compare(html, html, context);

        Assert.True(result.IsEqual);
    }
}
