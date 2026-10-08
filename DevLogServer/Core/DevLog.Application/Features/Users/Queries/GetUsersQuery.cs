using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Users.Result;

namespace DevLog.Application.Features.Users.Queries;

// admin panelde kullanıcıları ve rollerini listelemek için
public record GetUsersQuery : IRequest<BaseResult<List<GetUserListQueryResult>>>;
