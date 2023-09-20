using System;

namespace Idempotency.DTOs;

[Serializable]
public class PaymentResponse
{
    public string Id { get; set; }
    public long Amount { get; set; }
    public string Currency { get; set; }
    public string CardNumber { get; set; }
    public DateTime Created { get; set; }
}