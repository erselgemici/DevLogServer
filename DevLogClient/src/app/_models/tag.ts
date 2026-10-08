// etiket (konu): .NET, Angular, SQL...
export class TagDto {
  id;
  name;
  slug;
  // sadece etiket listesinde geliyor (o etikette kaç yazı var)
  blogCount?: number;
}
