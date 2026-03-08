using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.CreateCustomFieldDefinition;

internal sealed class CreateCustomFieldDefinitionCommandHandler : ICommandHandler<CreateCustomFieldDefinitionCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomFieldDefinitionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateCustomFieldDefinitionCommand request, CancellationToken cancellationToken)
    {
        bool fieldExists = await _context.CustomFieldDefinitions.AnyAsync(d => d.FieldName == request.FieldName, cancellationToken);
        if (fieldExists)
        {
            return Result.Failure<Guid>(Error.Conflict("CustomFieldDefinition.AlreadyExists", $"A custom field with name {request.FieldName} already exists."));
        }

        CustomFieldDefinition definition = new()
        {
            Id = Guid.NewGuid(),
            FieldName = request.FieldName,
            FieldLabel = request.FieldName, // Use FieldName as label for now
            DataType = request.FieldType,
            FieldType = request.FieldType,
            AppliesTo = "Trip", // Default to Trip
            IsRequired = request.IsRequired,
            DefaultValue = request.DefaultValue,
            ValidationRegex = request.ValidationRegex,
            IsActive = true,
            SortOrder = 0
        };

        _context.CustomFieldDefinitions.Add(definition);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(definition.Id);
    }
}
