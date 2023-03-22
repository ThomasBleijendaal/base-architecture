namespace API.Binding;

public class ValidatedModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType.IsGenericType
            && context.Metadata.ModelType.GetGenericTypeDefinition() == typeof(Validated<>))
        {
            if (context.Metadata.BindingSource == BindingSource.Body)
            {
                return (IModelBinder?)Activator.CreateInstance(typeof(ValidatedBodyModelBinder<>)
                    .MakeGenericType(context.Metadata.ModelType.GenericTypeArguments));
            }
            else if (context.Metadata.BindingSource == BindingSource.Query)
            {
                return (IModelBinder?)Activator.CreateInstance(typeof(ValidatedQueryModelBinder<>)
                    .MakeGenericType(context.Metadata.ModelType.GenericTypeArguments));
            }
        }

        return null;
    }
}
