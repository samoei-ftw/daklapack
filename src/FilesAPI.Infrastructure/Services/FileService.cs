using FilesAPI.Interfaces;

namespace FilesAPI.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;

        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
        }

        // Placeholder method for retrieving a file by name
        public async Task<byte[]> GetFileBytes(string fileName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Processes an uploaded file by copying its contents line-by-line to a temporary file,
        /// appending a new line at the end, then returning the final file as a byte array.
        /// </summary>
        /// <param name="file">The uploaded file to be processed.</param>
        /// <returns>Byte array containing the contents of the processed file.</returns>
        public async Task<byte[]> MutateFile(IFormFile file)
        {
            string filePath = string.Empty;

            try
            {
                // Validate input
                if (file == null)
                    throw new ArgumentNullException(nameof(file));

                // Generate a temporary file path on disk
                filePath = Path.GetTempFileName();

                // Open streams for reading from the input and writing to the temp file
                using (var stream = new FileStream(filePath, FileMode.Create))            // Create or overwrite the temp file
                using (var writer = new StreamWriter(stream))                             // Used to write lines to the file
                using (var reader = new StreamReader(file.OpenReadStream()))              // Read from the uploaded file's stream
                {
                    // Copy original file content line by line
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        await writer.WriteLineAsync(line);
                    }

                    // Append a custom line at the end
                    await writer.WriteLineAsync("This is a new line added to the file.");
                }

                _logger.LogInformation("File processed successfully.");

                // Return the entire file as byte array for download or further use
                return await File.ReadAllBytesAsync(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing the file.");
                return Array.Empty<byte>(); // Return empty byte array to avoid 500 error in controller
            }
            finally
            {
                // Ensure temp file is cleaned up to prevent clutter or disk bloat
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Failed to delete temporary file: {filePath}. Reason: {ex.Message}");
                    }
                }
            }
        }
    }
}