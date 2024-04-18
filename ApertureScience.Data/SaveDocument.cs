using ApertureScience.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApertureScience.Data
{
    public class SaveDocument
    {
        public static Dictionary<int, string> SaveImageAsync(FixedAssetDocument imageFile, string environment)//, string name, int type)
        {
            string subfolder;
            var urls = new Dictionary<int, string>();

            if (imageFile.FixedAssetImage != null)
            {
                string fileExtension = Path.GetExtension(imageFile.FixedAssetImage.FileName);
                var uploadsFolder = Path.Combine(environment, "FixedAssets", "Images");
                var uniqueFileName = "_" + imageFile.Name + fileExtension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //var imagePath = SaveDocument.SaveImageAsync(imageFile.FixedAssetImage, environment, "FixedAssetsImage");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    //imageFile.FixedAssetImage.CopyToAsync(fileStream);
                    imageFile.FixedAssetImage.CopyTo(fileStream);
                }
                urls.Add(1, Path.Combine("FixedAssets", "Images", uniqueFileName).Replace("\\", "/"));
            }

            if (imageFile.FixedAssetContract != null)
            {
                string fileExtension = Path.GetExtension(imageFile.FixedAssetContract.FileName);
                var uploadsFolder = Path.Combine(environment, "FixedAssets", "Contracts");
                var uniqueFileName = "_" + imageFile.Name + fileExtension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //var imagePath = SaveDocument.SaveImageAsync(imageFile.FixedAssetImage, environment, "FixedAssetsImage");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    //imageFile.FixedAssetImage.CopyToAsync(fileStream);
                    imageFile.FixedAssetContract.CopyTo(fileStream);
                }
                urls.Add(2,Path.Combine("FixedAssets", "Contracts", uniqueFileName).Replace("\\", "/"));
            }

            if (imageFile.FixedAssetWarranty != null)
            {
                string fileExtension = Path.GetExtension(imageFile.FixedAssetWarranty.FileName);
                var uploadsFolder = Path.Combine(environment, "FixedAssets", "Warrantys");
                var uniqueFileName = "_" + imageFile.Name + fileExtension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //var imagePath = SaveDocument.SaveImageAsync(imageFile.FixedAssetImage, environment, "FixedAssetsImage");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    //imageFile.FixedAssetImage.CopyToAsync(fileStream);
                    imageFile.FixedAssetWarranty.CopyTo(fileStream);
                }
                urls.Add(3,Path.Combine("FixedAssets", "Warrantys", uniqueFileName).Replace("\\", "/"));
            }

            return urls;
        }
    }
}
