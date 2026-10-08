using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Categories.Commands;

// kategori güncellemek için gelen model, Id ile hangi kayıt olduğunu buluyoruz
public record UpdateCategoryCommand(Guid Id, string CategoryName, string? CategoryNameEn = null) : IRequest<BaseResult<bool>>;