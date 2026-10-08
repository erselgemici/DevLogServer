using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Blogs.Result;

namespace DevLog.Application.Features.Blogs.Queries;

// arama kutusundan gelen kelimeye göre yazı arama
public record SearchBlogsQuery(string Text) : IRequest<BaseResult<List<GetBlogsQueryResult>>>;
