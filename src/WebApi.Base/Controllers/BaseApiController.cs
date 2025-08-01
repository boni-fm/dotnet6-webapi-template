using Microsoft.AspNetCore.Mvc;
using WebApi.Base.Models;

namespace WebApi.Base.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Creates a standardized success response
    /// </summary>
    /// <typeparam name="T">Type of data</typeparam>
    /// <param name="data">Response data</param>
    /// <param name="message">Success message</param>
    /// <returns>Standardized API response</returns>
    protected IActionResult CreateSuccessResponse<T>(T data, string message = "Success")
    {
        var response = new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
        return Ok(response);
    }

    /// <summary>
    /// Creates a standardized error response
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="statusCode">HTTP status code</param>
    /// <returns>Standardized API error response</returns>
    protected IActionResult CreateErrorResponse(string message, int statusCode = 400)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Data = null
        };

        return statusCode switch
        {
            400 => BadRequest(response),
            401 => Unauthorized(response),
            403 => StatusCode(403, response),
            404 => NotFound(response),
            500 => StatusCode(500, response),
            _ => BadRequest(response)
        };
    }

    /// <summary>
    /// Creates a standardized validation error response
    /// </summary>
    /// <returns>Standardized API validation error response</returns>
    protected IActionResult CreateValidationErrorResponse()
    {
        var errors = ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = "Validation failed",
            Data = errors
        };

        return BadRequest(response);
    }
}