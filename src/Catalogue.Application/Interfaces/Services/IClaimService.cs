using Catalogue.Application.Abstractions;
using System.Security.Claims;

namespace Catalogue.Application.Interfaces.Services;

public interface IClaimService
{
    public void AddRolesToClaims(IEnumerable<string> roles, List<Claim> claims);
    public IList<Claim> CreateAuthClaims<TUser>(TUser user) where TUser : UserBase; 
}

