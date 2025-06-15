namespace FilesAPI.Domain.File.Commands;

public class MutateFileCommand
{
    public IFormFile File { get; }

    public MutateFileCommand(IFormFile file)
    {
        File = file ?? throw new ArgumentNullException(nameof(file));
    }
}