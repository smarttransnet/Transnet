using SharedKernel;

namespace Application.Trailers;

public static class TrailerErrors
{
    public static Error NotFound(Guid trailerId) => Error.NotFound(
        "Trailers.NotFound",
        $"The trailer with the Id = '{trailerId}' was not found");
}
