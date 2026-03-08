using SharedKernel;

namespace Domain.Drivers;

public static class DriverErrors
{
    public static Error NotFound(Guid driverId) => Error.NotFound(
        "Drivers.NotFound",
        $"The driver with the Id = '{driverId}' was not found");
}
