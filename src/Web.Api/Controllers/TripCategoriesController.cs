#pragma warning disable S6960
using Application.Abstractions.Messaging;
using SharedKernel;
using Application.TripCategories;
using Application.TripCategories.CreateTripCategory;
using Application.TripCategories.DeleteTripCategory;
using Application.TripCategories.GetMaterialsLookup;
using Application.TripCategories.GetTripCategories;
using Application.TripCategories.GetTripCategoriesLookup;
using Application.TripCategories.GetTripCategoryById;
using Application.TripCategories.GetUomsLookup;
using Application.TripCategories.UpdateTripCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/trip-categories")]
public class TripCategoriesController(
    IQueryHandler<GetTripCategoriesQuery, PagedList<TripCategoryResponse>> getTripCategoriesHandler,
    IQueryHandler<GetTripCategoryByIdQuery, TripCategoryResponse> getTripCategoryByIdHandler,
    ICommandHandler<CreateTripCategoryCommand, List<Guid>> createTripCategoryHandler,
    ICommandHandler<UpdateTripCategoryCommand> updateTripCategoryHandler,
    ICommandHandler<DeleteTripCategoryCommand> deleteTripCategoryHandler,
    IQueryHandler<GetTripCategoriesLookupQuery, List<TripCategoryLookupResponse>> getTripCategoriesLookupHandler,
    IQueryHandler<GetUomsLookupQuery, List<UomLookupResponse>> getUomsLookupHandler
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? searchTerm,
        [FromQuery] bool? isActive,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = "asc",
        CancellationToken cancellationToken = default
    ) {
        var query = new GetTripCategoriesQuery(searchTerm, isActive, page, pageSize, sortBy, sortOrder);
        var result = await getTripCategoriesHandler.Handle(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTripCategoryByIdQuery(id);
        var result = await getTripCategoryByIdHandler.Handle(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTripCategoryCommand command, CancellationToken cancellationToken)
    {
        var result = await createTripCategoryHandler.Handle(command, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        var firstId = result.Value.Count > 0 ? result.Value[0] : Guid.Empty;
        return CreatedAtAction(nameof(GetById), new { id = firstId }, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTripCategoryRequestDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateTripCategoryCommand(
            id,
            dto.CategoryName,
            dto.UomIds,
            dto.NewUoms
        );
        var result = await updateTripCategoryHandler.Handle(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteTripCategoryCommand(id);
        var result = await deleteTripCategoryHandler.Handle(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    // Lookup Endpoints for Dropdowns

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategoriesLookup([FromQuery] bool? isActive, CancellationToken cancellationToken)
    {
        var query = new GetTripCategoriesLookupQuery(isActive);
        var result = await getTripCategoriesLookupHandler.Handle(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }



    [HttpGet("uoms")]
    public async Task<IActionResult> GetUomsLookup([FromQuery] bool? isActive, CancellationToken cancellationToken)
    {
        var query = new GetUomsLookupQuery(isActive);
        var result = await getUomsLookupHandler.Handle(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}

public class UpdateTripCategoryRequestDto
{
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }

    public List<Guid>? UomIds { get; set; }
    public List<NewUomDto>? NewUoms { get; set; }
}
