import { CategoryDto } from "./category";
import { TagDto } from "./tag";
import { CommentDto } from "./commentDto";
import { UserDto } from "./userDto";

export class BlogDto{
  id;
  title;
  coverImage;
  blogImage;
  description;
  // liste endpoint'lerinde içeriğin tamamı yerine kısa özet geliyor
  summary;
  // kaç dakikalık okuma, backend hesaplıyor
  readingMinutes;
  // detayda gelen etiketler
  tags: TagDto[];
  // admin formunda seçilen etiketlerin id'leri (create/update'te backend'e gidiyor)
  tagIds: string[] = [];
  titleEn;
  descriptionEn;
  categoryId;
  category: CategoryDto;
  userId;
  createdAt;
  updatedAt;
  user: UserDto;
  comments: CommentDto[]




}
