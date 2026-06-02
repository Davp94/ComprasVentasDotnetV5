using System;
using System.ComponentModel.DataAnnotations;

namespace ComprasVentas;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UniqueNameAttribute : ValidationAttribute
{
    private readonly string _fieldName;
    public UniqueNameAttribute(string fieldName)
    {
        _fieldName = fieldName;
        ErrorMessage = $"El valor para  '{_fieldName}' ya existe.";
    }
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value is null or "")
            return ValidationResult.Success;
        
        var checker = validationContext.GetService<IUniqueNameChecker>();

        var existName = Task.Run(() => 
            checker.ExistUsernameAsync(value.ToString()))
            .GetAwaiter().GetResult();
        return existName ? new ValidationResult(FormatErrorMessage(validationContext.DisplayName))
        : ValidationResult.Success;
    }


}
