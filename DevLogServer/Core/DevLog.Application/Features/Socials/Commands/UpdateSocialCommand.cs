using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Socials.Commands;

// sosyal medya linki güncellemek için gelen model, Id ile hangi kayıt olduğunu buluyoruz
public record UpdateSocialCommand(Guid Id, string Title, string Url, string Icon) : IRequest<BaseResult<object>>;
