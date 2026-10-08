namespace DevLog.Application.Features.Dashboard.Result;

// admin paneli açılınca gösterilen özet bilgiler
public class GetDashboardQueryResult
{
    public int BlogCount { get; set; }
    public int CategoryCount { get; set; }
    public int TagCount { get; set; }
    public int CommentCount { get; set; }
    public int UnreadMessageCount { get; set; }
    public int UserCount { get; set; }

    // grafik için: her kategoride kaç yazı var
    public List<DashboardItem> BlogsPerCategory { get; set; } = new();
    // en çok yazan yazarlar
    public List<DashboardAuthor> TopAuthors { get; set; } = new();
    // son gelen yorumlar
    public List<DashboardComment> LatestComments { get; set; } = new();
}

// küçük yardımcı class'lar, sadece dashboard'da kullanıldığı için aynı dosyaya yazdım
public class DashboardItem
{
    public string Name { get; set; }
    public int Count { get; set; }
}

public class DashboardAuthor
{
    public string FullName { get; set; }
    public string? ImageUrl { get; set; }
    public int BlogCount { get; set; }
}

public class DashboardComment
{
    public string FullName { get; set; }
    public string Body { get; set; }
    public DateTime CommentDate { get; set; }
    public Guid BlogId { get; set; }
    public string BlogTitle { get; set; }
}
