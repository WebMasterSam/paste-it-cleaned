namespace PasteItCleaned.Core.Entities
{
    public enum SourceType
    {
        Text = 0,
        Image = 1,

        Word = 10,
        Excel = 11,
        PowerPoint = 12,

        Rtf = 13,

        OpenOffice = 20,

        LibreOffice = 30,

        Google = 40,
        GoogleSheets = 41,
        GoogleDocs = 42,

        Web = 99,

        Other = 99999
    }
}
