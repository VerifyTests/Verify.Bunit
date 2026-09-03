/// <summary>
/// A bunit test class often lives in a razor code behind, and nesting its snapshots under a plain
/// .cs sibling that does not exist leaves them sitting flat in Solution Explorer.
/// See https://github.com/VerifyTests/Verify.Bunit/issues/108.
/// <para>
/// The nesting metadata comes from the Verify package and is applied during evaluation, since
/// evaluated items are what Solution Explorer reads. So this evaluates this very project through
/// the packages it references, rather than asserting anything about the running tests.
/// </para>
/// </summary>
public class FileNestingTests
{
    [Fact]
    public async Task SnapshotsNestUnderTheTestThatProducedThem()
    {
        var directory = AttributeReader.GetProjectDirectory();
        var nesting = await Nesting(Path.Combine(directory, "Tests.csproj"));

        // The regression: this test class sits in a code behind, so CodeBehindTests.cs never existed
        Assert.Contains("CodeBehindTests.Component.verified.html", nesting);
        Assert.Equal(
            "CodeBehindTests.razor.cs",
            nesting["CodeBehindTests.Component.verified.html"]);

        var flat = nesting
            .Where(_ => _.Value.Length == 0 ||
                        !File.Exists(Parent(directory, _.Key, _.Value)))
            .Select(_ => $"{_.Key} -> {(_.Value.Length == 0 ? "not nested" : _.Value)}")
            .ToList();

        Assert.True(
            flat.Count == 0,
            $"""
             Snapshots not nested under a file that exists:
             {string.Join(Environment.NewLine, flat)}
             """);
    }

    // DependentUpon is relative to the snapshot, not to the project
    static string Parent(string directory, string snapshot, string parent) =>
        Path.Combine(directory, Path.GetDirectoryName(snapshot)!, parent);

    static async Task<Dictionary<string, string>> Nesting(string projectPath)
    {
        var json = await Evaluate(projectPath);
        using var document = JsonDocument.Parse(json);
        var nesting = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var type in document.RootElement.GetProperty("Items").EnumerateObject())
        {
            foreach (var item in type.Value.EnumerateArray())
            {
                var identity = item.GetProperty("Identity").GetString()!;
                if (!identity.Contains(".verified.") &&
                    !identity.Contains(".received."))
                {
                    continue;
                }

                // The SDK globs a snapshot as well as Verify, and only Verify's item carries the
                // nesting, so an entry without it is the other item for a file already covered.
                if (item.TryGetProperty("DependentUpon", out var parent) &&
                    parent.GetString() is { Length: > 0 } value)
                {
                    nesting[identity] = value;
                }
                else
                {
                    nesting.TryAdd(identity, "");
                }
            }
        }

        return nesting;
    }

    static async Task<string> Evaluate(string projectPath)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var arguments = startInfo.ArgumentList;
        arguments.Add("msbuild");
        arguments.Add(projectPath);
        arguments.Add("-getItem:None");
        arguments.Add("-getItem:Content");
        arguments.Add("-nologo");

        using var process = Process.Start(startInfo)!;

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        var output = await outputTask;
        var error = await errorTask;

        if (process.ExitCode == 0 &&
            output.TrimStart().StartsWith('{'))
        {
            return output;
        }

        throw new($"Evaluation of {projectPath} failed:{Environment.NewLine}{output}{Environment.NewLine}{error}");
    }
}
