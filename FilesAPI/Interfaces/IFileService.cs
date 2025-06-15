namespace FilesAPI.Interfaces;

public interface IFileService
{
    Task<byte[]>  GetFileBytes(string fileName);
    Task<byte[]> MutateFile(IFormFile file);
}