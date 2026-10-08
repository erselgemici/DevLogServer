using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Categories.Commands
{
    // kategori eklerken client'tan gelen veriler (command)
    public record CreateCategoryCommand(string CategoryName, string? CategoryNameEn = null) : IRequest<BaseResult<object>>;

}
