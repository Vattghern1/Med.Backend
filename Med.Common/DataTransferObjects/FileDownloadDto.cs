namespace Med.Common.DataTransferObjects;

public class FileDownloadDto
{
    public required string Name { get; set; }
    public required string ContentType { get; set; }
    public required byte[] Content { get; set; }
}
