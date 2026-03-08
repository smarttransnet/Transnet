using SharedKernel;

namespace Application.WorkOrders;

public static class WorkOrderErrors
{
    public static Error NotFound(Guid workOrderId) => Error.NotFound(
        "WorkOrders.NotFound",
        $"The work order with the Id = '{workOrderId}' was not found");
}
