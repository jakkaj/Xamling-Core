//From: http://stackoverflow.com/questions/930433/apply-properties-values-from-one-object-to-another-of-the-same-type-automaticall
//user: http://stackoverflow.com/users/428886/azerothian
//with Modifications for PCL by Jordan Knight. 
using System;
using System.Linq;
using System.Reflection;

/// <summary>
/// A static class for reflection type functions
/// </summary>
public static class SimpleMapper
{
    /// <summary>
    /// Extension for 'Object' that copies the properties to a destination object.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    public static void CopyProperties(this object source, object destination)
    {
        // If any this null throw an exception
        if (source == null || destination == null)
            throw new Exception("Source or/and Destination Objects are null");
        // Getting the Types of the objects
        var typeDest = destination.GetType();
        var typeSrc = source.GetType();
        // Collect all the valid properties to map

        var results = from srcProp in typeSrc.GetRuntimeProperties()
                      let targetProperty = typeDest.GetRuntimeProperty(srcProp.Name)
                      where srcProp.CanRead
                            && targetProperty != null
                            && targetProperty.SetMethod != null
                            && targetProperty.CanWrite
                            && (targetProperty.SetMethod.Attributes & MethodAttributes.Static) == 0
                            && targetProperty.PropertyType.GetTypeInfo().IsAssignableFrom(srcProp.PropertyType.GetTypeInfo())
                      select new { sourceProperty = srcProp, targetProperty = targetProperty };
        //map the properties
        foreach (var props in results)
        {
            props.targetProperty.SetValue(destination, props.sourceProperty.GetValue(source, null), null);
        }
    }
}
