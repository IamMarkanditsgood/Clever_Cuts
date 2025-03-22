using System;

public static class ResourceEvents
{
    public static event Action<ResourceTypes, int> OnResourceModified;

    public static void ResourceModified(ResourceTypes resource, int newAmount)
    {
        OnResourceModified?.Invoke(resource, newAmount);
    }
}