public static class ModuleInitializer
{
    #region BunitEnable

    [ModuleInitializer]
    public static void Initialize() =>
        VerifyBunit.Initialize();

    #endregion

    [ModuleInitializer]
    public static void InitializeOther()
    {
        #region scrubbers

        // remove some noise from the html snapshot
        VerifierSettings.ScrubEmptyLines();
        BlazorScrubber.ScrubCommentLines();
        VerifierSettings.ScrubLinesWithReplace(
            line =>
            {
                var scrubbed = line.Replace("<!--!-->", "");
                if (string.IsNullOrWhiteSpace(scrubbed))
                {
                    return null;
                }

                return scrubbed;
            });
        HtmlPrettyPrint.All();
        VerifierSettings.ScrubLinesContaining("<script src=\"_framework/dotnet.");

        #endregion

        VerifierSettings.InitializePlugins();
    }
}