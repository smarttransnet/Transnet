using SharedKernel;

namespace Domain.Trips;

public static class TripErrors
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "Trip.NotFound",
        $"The trip with the identifier {id} was not found.");
}
