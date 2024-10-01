using AutoMapper;
using Catalogue.Application.Users.Queries.Requests;
using Catalogue.Application.Users.Queries.Responses;
using Catalogue.Application.Utils;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Interfaces;
using MediatR;

namespace Catalogue.Application.Users.Queries.Handlers;

public sealed class LoginRequestHandler : IRequestHandler<LoginQueryRequest, LoginQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private LoginQueryResponse response;

    public LoginRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        response = new LoginQueryResponse();    
    }

    public async Task<LoginQueryResponse> Handle(LoginQueryRequest request,
                                                 CancellationToken cancellationToken)
    {
        User? user = await _unitOfWork.UserRepository.GetAsync(u => u.Name == request.Name);

        if(user != null && user.Password == Crypto.Encrypt(request.Password)) 
        {
            response = _mapper.Map<LoginQueryResponse>(user);
            response.Success = true;
            return response;
        }

        response.Success = false;
        return response;
    }
}
