public class HtmlComparerTests
{
    // Regression for a NullReferenceException in AngleSharp.Diffing's StyleAttributeComparer. This style
    // attribute differs from the verified file's only in whitespace and a trailing semicolon, so the two
    // markup strings are not byte-equal and the html comparer parses and compares them. It used to throw
    // because the parser had no CSS support, leaving each element's inline style null; the styles must now
    // compare as semantically equal. The two spellings are deliberately different — keep them that way.
    [Fact]
    public Task StyledElement_SemanticallyEqual() =>
        Verify(
            "<div style=\"color:red;font-size:10pt;\">text</div>",
            extension: "html");
}
