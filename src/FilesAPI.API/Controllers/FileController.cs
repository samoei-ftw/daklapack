using FilesAPI.Domain;
using FilesAPI.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace FilesAPI.Controllers;

[ApiController] // enables web api behaviour
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly MutateFileCommandHandler _fileCommandHandler;
    private readonly ILogger<FileController> _logger;

    public FileController(MutateFileCommandHandler fileCommandHandler, ILogger<FileController> logger)
    {
        _fileCommandHandler = fileCommandHandler;
        _logger = logger;
    }

    [HttpPost("mutate", Name = "UploadAndMutateFile")]
    public async Task<IActionResult> UploadAndMutateFile([FromForm] MutateFileCommand mutateFileCommand)
    {
        if (mutateFileCommand.File == null)
            return BadRequest();

        var result = await _fileCommandHandler.Handle(mutateFileCommand);
        // return downloadable file
        return File(result, "application/octet-stream", $"mutated-{mutateFileCommand.File.FileName}");
    }
}