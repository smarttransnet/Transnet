using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.GetCustomFieldDefinitions;

internal sealed class GetCustomFieldDefinitionsQueryHandler : IQueryHandler<GetCustomFieldDefinitionsQuery, List<CustomFieldDefinitionResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetCustomFieldDefinitionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<CustomFieldDefinitionResponse>>> Handle(GetCustomFieldDefinitionsQuery request, CancellationToken cancellationToken)
    {
        List<CustomFieldDefinitionResponse> definitions = await _context.CustomFieldDefinitions
            .OrderBy(d => d.FieldName)
            .Select(d => new CustomFieldDefinitionResponse(
                d.Id,
                d.FieldName,
                d.FieldLabel,
                d.DataType,
                d.FieldType,
                d.AppliesTo,
                d.IsRequired,
                d.DefaultValue,
                d.ValidationRegex,
                d.IsActive,
                d.SortOrder))
            .ToListAsync(cancellationToken);

        return Result.Success(definitions);
    }
}
