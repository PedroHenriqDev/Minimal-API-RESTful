using Catalogue.Application.Abstractions;
using System.Security.Claims;

namespace Catalogue.Application.Interfaces.Services;

public interface IClaimService
{
    public List<Claim> CreateAuthClaims<TUser>(TUser user) where TUser : UserBase;
}

