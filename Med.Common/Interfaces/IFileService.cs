using Med.Common.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace Med.Common.Interfaces;

public interface IFileService
{
    public Task<List<FileKeyDto>> UploadFiles(List<IFormFile> files);
    public Task RemoveFiles(List<string> fileIds);
    public Task<string?> GetAvatarLink(string avatarId);
    public Task<string> GetFileLink(string fileId);
    public Task<List<FileDownloadDto>> GetFiles(List<string> fileNames);
    public Task CreateBuckets();
}