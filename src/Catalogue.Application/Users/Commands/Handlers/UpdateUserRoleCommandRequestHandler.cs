using AutoMapper;
using Catalogue.Application.Exceptions;
using Catalogue.Application.Resources;
using Catalogue.Application.Users.Commands.Requests;
using Catalogue.Application.Users.Commands.Responses;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Enums;
using Catalogue.Domain.Interfaces;
using MediatR;

namespace Catalogue.Application.Users.Commands.Handlers;

internal class UpdateUserRoleCommandRequestHandler :
    IRequestHandler<UpdateUserRoleCommandRequest, UpdateUserRoleCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private string notFoundMessage = string.Empty; 

    public UpdateUserRoleCommandRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }

    public async Task<UpdateUserRoleCommandResponse> Handle(UpdateUserRoleCommandRequest request, 
                                                            CancellationToken cancellationToken)
    {
        if(!Enum.TryParse(typeof(Role), request.RoleName, out var role) || role == null)
        {
            notFoundMessage = string.Format(ErrorMessagesResource.NOT_FOUND_ROLE, request.RoleName);
            throw new NotFoundException(notFoundMessage);
        }

        User? user = await _unitOfWork.UserRepository.GetAsync(u => u.Id == request.Id);
        if (user == null) 
        {
            notFoundMessage = string.Format(ErrorMessagesResource.NOT_FOUND_ID_MESSAGE, typeof(User).Name, request.Id);
            throw new NotFoundException(notFoundMessage);
        }

        user.Role = (Role)role;
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<UpdateUserRoleCommandResponse>(user);
    }
}
