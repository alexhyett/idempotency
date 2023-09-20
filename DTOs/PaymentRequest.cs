using System;

namespace Idempotency.DTOs;

[Serializable]
public class PaymentRequest
{
    public long Amount { get; set; }
    public string Currency { get; set; }
    public string CardNumber { get; set; }
    public string CVV { get; set; }
    public string ExpiryYear { get; set; }
    public string ExpiryMonth { get; set; }
}

