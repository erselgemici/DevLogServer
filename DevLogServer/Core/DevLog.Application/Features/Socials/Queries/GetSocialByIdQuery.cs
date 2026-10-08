using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Socials.Result;

namespace DevLog.Application.Features.Socials.Queries;

// tek bir sosyal medya linki getirmek için sorgu
public record GetSocialByIdQuery(Guid Id) : IRequest<BaseResult<GetSocialByIdQueryResult>>;
