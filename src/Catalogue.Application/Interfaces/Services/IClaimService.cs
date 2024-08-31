using Catalogue.Domain.Entities;
using System.Security.Claims;

namespace Catalogue.Application.Interfaces.Services;

public interface IClaimService
{
    public void AddRolesToClaims(IEnumerable<string> roles, List<Claim> claims);
    public List<Claim> CreateAuthClaims(User user); 
}
