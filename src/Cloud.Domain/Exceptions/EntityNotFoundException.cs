namespace Cloud.Domain.Exceptions;

using System.Diagnostics.CodeAnalysis;

[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors.", Justification = "Base class constructors and properties aren't meant to be used. ")]
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string? entityID, string? entityTypeName)
    {
        ArgumentException.ThrowIfNullOrEmpty(entityID);
        ArgumentException.ThrowIfNullOrEmpty(entityTypeName);

        this.EntityID = entityID;
        this.EntityTypeName = entityTypeName;
    }

    public string EntityID { get; }
    public string EntityTypeName { get; }
}
