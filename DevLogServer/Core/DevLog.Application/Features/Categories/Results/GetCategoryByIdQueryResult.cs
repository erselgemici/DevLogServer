using DevLog.Application.Base;

namespace DevLog.Application.Features.Categories.Result
{
    // client'a döndüğüm dto, entity'i direkt dışarı açmamak için ayrı class yaptım
    public class GetCategoryByIdQueryResult : BaseDto
    {
        public string CategoryName { get; set; }
        public string? CategoryNameEn { get; set; }
        //public IList<GetBlogQueryResult> Blogs { get; set; }
    }
}
