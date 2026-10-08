using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Socials.Commands;

// sosyal medya linki eklerken client'tan gelen veriler (command)
public record CreateSocialCommand : IRequest<BaseResult<object>>
{
    public string Title { get; init; }
    public string Url { get; init; }
    public string Icon { get; init; }
}
