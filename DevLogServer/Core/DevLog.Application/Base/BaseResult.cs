using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace DevLog.Application.Base;

// bütün endpointler aynı formatta cevap dönsün diye yazdığım generic sonuç class'ı
public class BaseResult<T>
{
    public T? Data { get; set; }

    public IEnumerable<object>? Errors { get; set; }

    [JsonIgnore]
    // hata listesi boşsa işlem başarılı sayılıyor
    public bool IsSuccess => Errors == null || !Errors.Any();

    [JsonIgnore]
    public bool IsFailure => !IsSuccess;

    public static BaseResult<T> Success(T data)
    {
        return new BaseResult<T> { Data = data };
    }

    public static BaseResult<T> Fail(string error)
    {
        return new BaseResult<T>
        {
            Errors = [new Error { ErrorMessage = error }]
        };
    }

    public static BaseResult<T> Fail()
    {
        return new BaseResult<T>
        {
            Errors = [new Error { ErrorMessage = "Beklenmeyen bir hata oluştu" }]
        };
    }

    // identity'nin hatalarını kendi Error formatıma çeviriyorum (register'da kullanılıyor)
    public static BaseResult<T> Fail(IEnumerable<IdentityError> errors)
    {
        return new BaseResult<T>
        {
            Errors = (from error in errors
                      select new Error { PropertyName = error.Code, ErrorMessage = error.Description }).ToList()
        };
    }

    public static BaseResult<T> Fail(IEnumerable<string> errors)
    {
        return new BaseResult<T>
        {
            Errors = (from error in errors
                      select new Error { ErrorMessage = error }).ToList()
        };
    }

    // kayıt bulunamadığında kullanıyorum
    public static BaseResult<T> NotFound(string message)
    {
        return new BaseResult<T>
        {
            Errors = [new Error { ErrorMessage = message }]
        };
    }
}

public class Error
{
    public string? PropertyName { get; set; }
    public string ErrorMessage { get; set; }
}
