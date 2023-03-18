using Common.Validation;

namespace Common.Binding;

public interface IModelBinder<T>
{
    ValueTask<Validated<T>?> BindAndValidateAsync(HttpContext context, ParameterInfo parameter);
}
