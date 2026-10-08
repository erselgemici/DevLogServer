using DevLog.Application.Base;
using DevLog.Application.Features.Blogs.Result;

namespace DevLog.Application.Features.Categories.Result
{
    // client'a döndüğüm dto, entity'i direkt dışarı açmamak için ayrı class yaptım
    public class GetCategoryQueryResult : BaseDto
    {
        public string CategoryName { get; set; }
        public string? CategoryNameEn { get; set; }
        public IList<GetBlogsQueryResult> Blogs { get; set; }
    }
}
