using GoogleExplorer.Extensions;

namespace GoogleExplorer
{
    public enum MimeTypes
    {
        [MimeType("")]
        Null,
        [MimeType("application/vnd.google-apps.folder")]
        GoogleFolder,
        [MimeType("application/vnd.google-apps.spreadsheet")]
        GoogleSpreadSheet,
        [MimeType("text/plain")]
        Text,
        [MimeType("application/pdf")]
        PDF,
        [MimeType("aplication/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        OfficeWord,
        [MimeType("image/jpeg")]
        JPG,
        [MimeType("image/png")]
        PNG,

    }

    public enum RequestResult
    {
        Cancelled, TokenExpired, Success, Error
    }
}
