namespace SmellFinderTool.Core.Models
{
    public static class MessageFormatterModel
    {
        #region Methods
        public static string GetFormattedText(string format, string text) => $"[{format}]{text}[/]";
        #endregion
    }
}