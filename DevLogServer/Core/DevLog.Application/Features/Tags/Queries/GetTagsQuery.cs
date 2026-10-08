using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Tags.Result;

namespace DevLog.Application.Features.Tags.Queries;

// bütün etiketleri yazı sayılarıyla getiren sorgu
public record GetTagsQuery : IRequest<BaseResult<List<GetTagsQueryResult>>>;
