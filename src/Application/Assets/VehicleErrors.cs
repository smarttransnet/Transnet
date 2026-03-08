using SharedKernel;

namespace Application.Assets;

public static class VehicleErrors
{
    public static Error NotFound(Guid vehicleId) => Error.NotFound(
        "Vehicles.NotFound",
        $"The vehicle with the Id = '{vehicleId}' was not found");
}
