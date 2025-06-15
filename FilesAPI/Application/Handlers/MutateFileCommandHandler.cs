using FilesAPI.Domain.File.Commands;
using FilesAPI.Interfaces;

namespace FilesAPI.Application.File.Handlers;

public class MutateFileCommandHandler
{
    private readonly IFileService _fileMutationService;

    public MutateFileCommandHandler(IFileService fileMutationService)
    {
        _fileMutationService = fileMutationService;
    }

    public async Task<byte[]> Handle(MutateFileCommand command)
    {
        return await _fileMutationService.MutateFile(command.File);
    }
}