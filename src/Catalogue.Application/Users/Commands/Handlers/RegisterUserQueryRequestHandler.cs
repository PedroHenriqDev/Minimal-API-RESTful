using AutoMapper;
using Catalogue.Application.Users.Commands.Requests;
using Catalogue.Application.Users.Commands.Responses;
using Catalogue.Application.FluentValidation;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Catalogue.Application.Resources;
using Catalogue.Application.Exceptions;
using Catalogue.Application.Utils;

namespace Catalogue.Application.Users.Commands.Handlers;

public sealed class RegisterUserQueryRequestHandler : IRequestHandler<RegisterUserCommandRequest, RegisterUserCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<RegisterUserCommandRequest> _validator;

    public RegisterUserQueryRequestHandler(IUnitOfWork unitOfWork,
                                      IMapper mapper,
                                      IValidator<RegisterUserCommandRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommandRequest request,
                                                    CancellationToken cancellationToken)
    {
        _validator.EnsureValid(request);

        if (await _unitOfWork.UserRepository
            .GetAsNoTrackingAsync(u => u.Name!.ToLower() == request.Name!.ToLower()) != null)
        {
            string existsMessage = string.Format(ErrorMessagesResource.NAME_EXISTS_MESSAGE, request.Name);
            throw new ExistsValueException(existsMessage);
        }

        var userToRegister = _mapper.Map<User>(request);

        await _unitOfWork.UserRepository.AddAsync(userToRegister);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<RegisterUserCommandResponse>(userToRegister);
    }
}
