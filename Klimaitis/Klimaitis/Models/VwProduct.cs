using System;
using System.Collections.Generic;

namespace Klimaitis.Models;

public partial class VwProduct
{
    public int ProductId { get; set; }

    public string Article { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal Price { get; set; }

    public string? SupplierName { get; set; }

    public string? ManufacturerName { get; set; }

    public string? CategoryName { get; set; }

    public decimal Discount { get; set; }

    public int QuantityInStock { get; set; }

    public string? Description { get; set; }

    public string? PhotoPath { get; set; }

    public decimal? PriceWithDiscount { get; set; }
}
