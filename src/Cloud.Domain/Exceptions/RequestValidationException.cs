namespace Cloud.Domain.Exceptions;

using System.Diagnostics.CodeAnalysis;

[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors.", Justification = "Base class constructors and properties aren't meant to be used. ")]
public class RequestValidationException : Exception
{
    public RequestValidationException(Dictionary<string, string[]> entityErrorsByProperty)
    {
        ArgumentNullException.ThrowIfNull(entityErrorsByProperty);

        this.EntityErrorsByProperty = entityErrorsByProperty;
    }


    public Dictionary<string, string[]> EntityErrorsByProperty { get; }
}
