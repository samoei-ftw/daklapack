using FilesAPI.Application.File.Handlers;
using FilesAPI.Domain.File.Commands;
using FilesAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FilesAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly MutateFileCommandHandler _handler;
    private readonly ILogger<FileController> _logger;

    public FileController(MutateFileCommandHandler handler, ILogger<FileController> logger)
    {
        _handler = handler;
        _logger = logger;
    }

    [HttpPost("mutate", Name = "UploadAndMutateFile")]
    public async Task<IActionResult> UploadAndMutateFile(IFormFile file)
    {
        if (file == null)
            return BadRequest("File is required.");

        var command = new MutateFileCommand(file);
        var result = await _handler.Handle(command);

        if (result.Length == 0)
            return StatusCode(500, "Could not process file.");

        return File(result, "application/octet-stream", $"mutated-{file.FileName}");
    }
}