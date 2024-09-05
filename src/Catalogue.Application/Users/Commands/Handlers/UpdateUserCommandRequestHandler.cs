using AutoMapper;
using Catalogue.Application.Exceptions;
using Catalogue.Application.FluentValidation;
using Catalogue.Application.Resources;
using Catalogue.Application.Users.Commands.Requests;
using Catalogue.Application.Users.Commands.Responses;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace Catalogue.Application.Users.Commands.Handlers;

public sealed class UpdateUserCommandRequestHandler :
    IRequestHandler<UpdateUserCommandRequest, UpdateUserCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateUserCommandRequest> _validator;

    public UpdateUserCommandRequestHandler(IUnitOfWork unitOfWork, 
                                           IMapper mapper, 
                                           IValidator<UpdateUserCommandRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommandRequest request, 
                                                        CancellationToken cancellationToken)
    {
        if(await _unitOfWork.UserRepository.GetAsync(u => u.Name == request.Name) is User user)
        {
            _validator.EnsureValid(request);

            _mapper.Map(request, user);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<UpdateUserCommandResponse>(user);
        }

        throw new AuthenticationUserException(ErrorMessagesResource.AUTHENTICATION_ERROR);
    }
}
