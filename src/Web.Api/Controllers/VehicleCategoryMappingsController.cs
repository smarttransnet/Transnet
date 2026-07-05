#pragma warning disable S6960
using Application.Abstractions.Messaging;
using SharedKernel;
using Application.VehicleCategoryMappings;
using Application.VehicleCategoryMappings.CreateMapping;
using Application.VehicleCategoryMappings.DeleteMapping;
using Application.VehicleCategoryMappings.GetMappings;
using Application.VehicleCategoryMappings.GetMappingById;
using Application.VehicleCategoryMappings.GetUomsLookup;
using Application.VehicleCategoryMappings.UpdateMapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/vehicle-category-mappings")]
public class VehicleCategoryMappingsController(
    IQueryHandler<GetVehicleCategoryMappingsQuery, PagedList<VehicleCategoryMappingResponse>> getMappingsHandler,
    IQueryHandler<GetVehicleCategoryMappingByIdQuery, VehicleCategoryMappingResponse> getMappingByIdHandler,
    ICommandHandler<CreateVehicleCategoryMappingCommand, List<Guid>> createMappingHandler,
    ICommandHandler<UpdateVehicleCategoryMappingCommand> updateMappingHandler,
    ICommandHandler<DeleteVehicleCategoryMappingCommand> deleteMappingHandler,
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
        var query = new GetVehicleCategoryMappingsQuery(searchTerm, isActive, page, pageSize, sortBy, sortOrder);
        var result = await getMappingsHandler.Handle(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetVehicleCategoryMappingByIdQuery(id);
        var result = await getMappingByIdHandler.Handle(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVehicleCategoryMappingCommand command, CancellationToken cancellationToken)
    {
        var result = await createMappingHandler.Handle(command, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        // Since we return a list of mapping IDs but the GetById is by CategoryId, 
        // we can just return Ok or Created.
        return Ok(result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateVehicleCategoryMappingRequestDto dto, CancellationToken cancellationToken)
    {
        // id here is the VehicleCategoryId
        var command = new UpdateVehicleCategoryMappingCommand(
            id,
            dto.UomIds,
            dto.NewUoms
        );
        var result = await updateMappingHandler.Handle(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteVehicleCategoryMappingCommand(id);
        var result = await deleteMappingHandler.Handle(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    [HttpGet("uoms")]
    public async Task<IActionResult> GetUomsLookup([FromQuery] bool? isActive, CancellationToken cancellationToken)
    {
        var query = new GetUomsLookupQuery(isActive);
        var result = await getUomsLookupHandler.Handle(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}

public class UpdateVehicleCategoryMappingRequestDto
{
    public List<Guid>? UomIds { get; set; }
    public List<NewUomDto>? NewUoms { get; set; }
}
