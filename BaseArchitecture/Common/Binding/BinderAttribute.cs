namespace Common.Binding;

[AttributeUsage(AttributeTargets.Parameter)]
public class BinderAttribute : Attribute
{
    public BinderAttribute(
        Type modelBinderType)
    {
        ModelBinderType = modelBinderType;
    }

    public Type ModelBinderType { get; }
}
