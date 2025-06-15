using System.Diagnostics;
using FilesAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FilesAPI.Models;

namespace FilesAPI.Controllers;

[ApiController] // enables web api behaviour
[Route("api/[controller]")]
public class FileController : ControllerBase // controllerBase for
{
    private readonly IFileService _fileService;
    private readonly ILogger<FileController> _logger;

    public FileController(IFileService fileService, ILogger<FileController> logger)
    {
        _fileService = fileService;
        _logger = logger;
    }

    [HttpPost(Name = "UploadFile")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        // validation
        if (file == null)
            return BadRequest();

        try
        {
            var fileBytes = await _fileService.MutateFile(file);
            return File(fileBytes, "application/octet-stream", $"mutated-{file.FileName}");
        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }
}