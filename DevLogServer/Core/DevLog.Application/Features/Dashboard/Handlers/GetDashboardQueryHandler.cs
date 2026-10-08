using MediatR;
using Microsoft.AspNetCore.Identity;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Dashboard.Queries;
using DevLog.Application.Features.Dashboard.Result;
using DevLog.Application.Localization;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Dashboard.Handlers;

// GetDashboardQuery gönderilince MediatR bu handler'ı çalıştırıyor
// birden fazla tablodan veri lazım olduğu için birden fazla repository inject ettim
public class GetDashboardQueryHandler(
    IRepository<Blog> blogRepository,
    IRepository<Category> categoryRepository,
    IRepository<Tag> tagRepository,
    IRepository<Comment> commentRepository,
    IRepository<Message> messageRepository,
    UserManager<AppUser> userManager)
    : IRequestHandler<GetDashboardQuery, BaseResult<GetDashboardQueryResult>>
{
    public Task<BaseResult<GetDashboardQueryResult>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        // NOT: sayılar için kayıtları çekmiyorum, Count() direkt SQL'de COUNT(*) çalıştırıyor
        var result = new GetDashboardQueryResult
        {
            BlogCount = blogRepository.GetQuery().Count(),
            CategoryCount = categoryRepository.GetQuery().Count(),
            TagCount = tagRepository.GetQuery().Count(),
            CommentCount = commentRepository.GetQuery().Count(),
            UnreadMessageCount = messageRepository.GetQuery().Count(x => !x.IsRead),
            UserCount = userManager.Users.Count()
        };

        // her kategoride kaç yazı var: GroupBy ile SQL'de GROUP BY CategoryId + COUNT
        var counts = blogRepository.GetQuery()
            .GroupBy(x => x.CategoryId)
            .Select(g => new { CategoryId = g.Key, Count = g.Count() })
            .ToDictionary(x => x.CategoryId, x => x.Count);

        // kategori adı dile göre geldiği için (çeviri tablosu) kategorileri çevirileriyle çekiyorum
        // sonra sayıları sözlükten (Dictionary) buluyorum, yazısı olmayan kategoride 0
        result.BlogsPerCategory = categoryRepository.GetQueryWithIncludes("Translations")
            .ToList()
            .Select(x => new DashboardItem
            {
                Name = x.GetCategoryName(LanguageContext.Current),
                Count = counts.GetValueOrDefault(x.Id)
            })
            .OrderByDescending(x => x.Count)
            .ToList();

        result.TopAuthors = userManager.Users
            .Select(x => new DashboardAuthor
            {
                FullName = x.FirstName + " " + x.LastName,
                ImageUrl = x.ImageUrl,
                BlogCount = x.Blogs.Count
            })
            .OrderByDescending(x => x.BlogCount)
            .Take(5)
            .ToList();

        // son 5 yorum, hangi yazıya yapıldığıyla beraber
        result.LatestComments = commentRepository.GetQueryWithIncludes("Blog", "Blog.Translations")
            .OrderByDescending(x => x.CommentDate)
            .Take(5)
            .ToList()
            .Select(x => new DashboardComment
            {
                FullName = x.FirstName + " " + x.LastName,
                Body = x.Body,
                CommentDate = x.CommentDate,
                BlogId = x.BlogId,
                BlogTitle = x.Blog.GetTitle(LanguageContext.Current)
            })
            .ToList();

        return Task.FromResult(BaseResult<GetDashboardQueryResult>.Success(result));
    }
}
