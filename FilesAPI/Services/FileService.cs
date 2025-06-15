using FilesAPI.Interfaces;

namespace FilesAPI.Services;

public class FileService: IFileService
{
    private readonly ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
    }
    public async Task<byte[]> GetFileBytes(string fileName)
    {
        throw new NotImplementedException();
    }

    public async Task<byte[]> MutateFile(IFormFile file)
    {
        try
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var filePath = Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            using (var writer = new StreamWriter(stream))
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    await writer.WriteLineAsync(line);
                }

                await writer.WriteLineAsync("This is a new line added to the file.");
            }

            _logger.LogInformation("File processed");

            return await File.ReadAllBytesAsync(filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while processing the file");
            return Array.Empty<byte>();
        }
    }
}