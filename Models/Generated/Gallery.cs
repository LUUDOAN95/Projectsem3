using System;
using System.Collections.Generic;

namespace eproject3.Models.Generated;

public partial class Gallery
{
    public int GalleryId { get; set; }

    public string? Title { get; set; }

    public string ImageUrl { get; set; } = null!;

    public DateTime? UploadDate { get; set; }
}
