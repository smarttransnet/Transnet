using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.UpdateCustomFieldDefinition;

internal sealed class UpdateCustomFieldDefinitionCommandHandler : ICommandHandler<UpdateCustomFieldDefinitionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCustomFieldDefinitionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateCustomFieldDefinitionCommand request, CancellationToken cancellationToken)
    {
        CustomFieldDefinition? definition = await _context.CustomFieldDefinitions
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (definition is null)
        {
            return Result.Failure(Error.NotFound("CustomFieldDefinition.NotFound", $"Custom field definition with ID {request.Id} was not found."));
        }

        definition.FieldName = request.FieldName;
        definition.FieldLabel = request.FieldName; // Use FieldName as label for now
        definition.DataType = request.FieldType;
        definition.FieldType = request.FieldType;
        definition.IsRequired = request.IsRequired;
        definition.DefaultValue = request.DefaultValue;
        definition.ValidationRegex = request.ValidationRegex;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
