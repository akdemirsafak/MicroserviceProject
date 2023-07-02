﻿using Web.Models.PhotoStocks;

namespace Web.Services.Interfaces;

public interface IPhotoStockService
{
    Task<PhotoViewModel> UploadPhoto(IFormFile photo);
    Task<bool> DeletePhoto(string photoUrl);
}
