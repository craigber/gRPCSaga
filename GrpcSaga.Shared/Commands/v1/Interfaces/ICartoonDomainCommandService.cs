using CartoonDomain.Shared.Commands.v1.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartoonDomain.Shared.Commands.v1.Interfaces;

public interface ICartoonDomainCommandService
{
    Task<CartoonUpdateResponse> UpdateCartoonAsync(CartoonUpdateRequest request);
}
