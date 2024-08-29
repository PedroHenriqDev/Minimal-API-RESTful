﻿using Catalogue.Application.Abstractions.Validators;
using Catalogue.Application.Products.Commands.Requests;

namespace Catalogue.Application.Validators.Products;

public class CreateProductCommandValidator : ProductBaseValidator<CreateProductCommandRequest>
{
}
