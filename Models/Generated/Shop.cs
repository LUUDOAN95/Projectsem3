using System;
using System.Collections.Generic;

namespace eproject3.Models.Generated;

public partial class Shop
{
    public int ShopId { get; set; }

    public string ShopName { get; set; } = null!;

    public string? Category { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsDeleted { get; set; }
}
