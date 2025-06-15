namespace FilesAPI.Domain;

public class MutateFileCommand
{
    public IFormFile File { get; set; }

    public MutateFileCommand(IFormFile file)
    {
        File = file ?? throw new ArgumentNullException(nameof(file));
    }
}