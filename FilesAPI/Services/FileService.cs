using FilesAPI.Interfaces;

namespace FilesAPI.Services;

public class FileService: IFileService
{
    private readonly ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
    }
/// <summary>
/// Mutates an uploaded file by copying the file's contents
/// line-by-line to a temporary file, adding a new line at the end
/// and then returning the final file as a byte array
/// </summary>
/// <param name="file"></param>
/// <returns></returns>
/// <exception cref="ArgumentNullException"></exception>
    public async Task<byte[]> MutateFile(IFormFile file)
    {
        try
        {
            // Validation
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            // Get temporary file path on disk
            var filePath = Path.GetTempFileName();
            
            // Open streams for reading and writing
            using (var stream = new FileStream(filePath, FileMode.Create))
            // FileMode.Create: if file exists, overwrite, else create    
            using (var writer = new StreamWriter(stream))
            // Open separate stream for reading (SOC)    
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                // Copy file's contents, line by line
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
            _logger.LogError(ex, $"An error occured while processing the file");
            return Array.Empty<byte>();
        }
    }
}