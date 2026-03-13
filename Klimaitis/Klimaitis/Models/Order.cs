using System;
using System.Collections.Generic;

namespace Klimaitis.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public int PickupPointId { get; set; }

    public int UserId { get; set; }

    public string PickupCode { get; set; } = null!;

    public int StatusId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual PickupPoint PickupPoint { get; set; } = null!;

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
