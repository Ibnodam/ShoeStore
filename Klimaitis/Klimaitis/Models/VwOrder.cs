using System;
using System.Collections.Generic;

namespace Klimaitis.Models;

public partial class VwOrder
{
    public int OrderId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public string? PickupAddress { get; set; }

    public string? CustomerFullName { get; set; }

    public string PickupCode { get; set; } = null!;

    public string? StatusName { get; set; }
}
