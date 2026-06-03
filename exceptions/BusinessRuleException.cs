using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;

namespace ComprasVentas;

public class BusinessRuleException : DomainException
{
    public BusinessRuleException(string message) : base(message, 422, "BUSINESS_RULE_VIOLATION"){}
    
}
