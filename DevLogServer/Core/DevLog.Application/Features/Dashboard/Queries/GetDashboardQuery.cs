using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Dashboard.Result;

namespace DevLog.Application.Features.Dashboard.Queries;

// admin paneli ana sayfası için istatistikler
public record GetDashboardQuery : IRequest<BaseResult<GetDashboardQueryResult>>;
