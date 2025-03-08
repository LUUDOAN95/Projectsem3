using System;
using System.Collections.Generic;

namespace eproject3.Models.Generated;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public string? CustomerName { get; set; }

    public string? Email { get; set; }

    public string? Message { get; set; }

    public DateTime? SubmissionDate { get; set; }
}
