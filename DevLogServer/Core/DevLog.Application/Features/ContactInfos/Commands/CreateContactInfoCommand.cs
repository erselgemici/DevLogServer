using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.ContactInfos.Commands;

// iletişim bilgisi eklerken client'tan gelen veriler (command)
public record CreateContactInfoCommand : IRequest<BaseResult<object>>
{
    public string Address { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string MapUrl { get; init; }
}
