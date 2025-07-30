using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace Shows.Api.Shows;

public class Controller(IDocumentSession session) : ControllerBase
{
    [HttpPost("/api/shows")]
    public async Task<ActionResult> AddAShowAsync(
        [FromBody] CreateShowRequest request,
        //[FromServices] IValidator<CreateVendorRequest> validator,
        CancellationToken token)
    {
        //if(!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }

        //var validationResults = await validator.ValidateAsync(request);
        //if (!validationResults.IsValid)
        //{
        //    return BadRequest(validationResults);
        //}
        // validation
        // You can't add a vendor with the same name more than once.
        // field validation - what is required, what is optional, what are the rules for the required things
        // domain validation - we don't already have a vendor with that same name
        // 
        // we have to "save it" somewhere. 

        // Mapping Code (copy from one object to another)
        var response = new CreateShowResponse(
            Guid.NewGuid(),
            request.Description,
            request.Name,
            request.StreamingService
            );
        session.Store(response);
        await session.SaveChangesAsync();
        return Ok(response);
    }

    public record CreateShowRequest
    {
        public string Description { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string StreamingService { get; init; } = string.Empty;
    }

    public record CreateShowResponse(
        Guid Id,
        string Description, string Name, string StreamingService
    );
}