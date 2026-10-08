// admin paneli ana sayfasındaki istatistikler (backend'deki GetDashboardQueryResult ile aynı alanlar)
export class DashboardDto {
  blogCount: number;
  categoryCount: number;
  tagCount: number;
  commentCount: number;
  unreadMessageCount: number;
  userCount: number;
  blogsPerCategory: { name: string, count: number }[] = [];
  topAuthors: { fullName: string, imageUrl: string, blogCount: number }[] = [];
  latestComments: { fullName: string, body: string, commentDate: string, blogId: string, blogTitle: string }[] = [];
}
