using Microsoft.AspNetCore.Mvc;
using PhotoStock.API.Dtos;
using SharedLibrary.ControllerBases;
using SharedLibrary.Dtos;

namespace PhotoStock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            //Parametre olarak cancellationToken'ı parametre olarak geçtiğimizde otomatik tetiklenir.
            //Async başlayan bir işlem sadece hata fırlatarak sonlandırılabilir.CancellationToken'da bu mantıkla çalışır.
            if (photo is not null && photo.Length > 0)
            {
                var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos",photo.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);
                var returnPath="photos/"+photo.FileName; //bu endpoint'in response u görselin adresi olacak.
                PhotoDto returnPathDto=new() { Url=returnPath};
                return CreateActionResult(Response<PhotoDto>.Success(returnPathDto, 201));
            }
            return CreateActionResult(Response<PhotoDto>.Fail("Photo Empty", 400));
        }

        public IActionResult PhotoDelete(string photoUrl)
        {
            var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos",photoUrl);

            if (!System.IO.File.Exists(path))
            {
                return CreateActionResult(Response<NoContent>.Fail("Görsel bulunamadı", 404));
            }
            System.IO.File.Delete(path);
            return CreateActionResult(Response<NoContent>.Success(204));
        }
    }
}
