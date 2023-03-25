﻿using FluentValidation;

namespace API.Models;

public class GetPokémonsRequestModelValidator : AbstractValidator<GetPokémonsRequestModel>
{
    public GetPokémonsRequestModelValidator()
    {
        RuleFor(m => m.Type).GreaterThanOrEqualTo(0).LessThanOrEqualTo(10);
    }
}
