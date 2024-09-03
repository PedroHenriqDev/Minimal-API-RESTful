using Catalogue.Application.Abstractions;
using System.Security.Claims;

namespace Catalogue.Application.Interfaces.Services;

public interface IClaimService
{
    public void AddRoleToClaims(string role, List<Claim> claims);
    public List<Claim> CreateAuthClaims<TUser>(TUser user) where TUser : UserBase;
}

