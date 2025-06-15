namespace FilesAPI.Interfaces;

public interface IFileService
{
    Task<byte[]> MutateFile(IFormFile file);
}