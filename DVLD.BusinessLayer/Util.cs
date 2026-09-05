using DVLD.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public static class Util
    {
        private static bool _CopyImageToProjectImageFolder(ref string sourceImageFile)
        {
            try
            {
                string imageFolder = Path.Combine
                    (AppDomain.CurrentDomain.BaseDirectory,
                     "Images", "Persons");

                Directory.CreateDirectory(imageFolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(sourceImageFile);

                string destinationPath = Path.Combine(imageFolder, fileName);

                File.Copy(sourceImageFile, destinationPath);

                sourceImageFile = Path.Combine("Images", "Persons", fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Result<string> _SaveImage(string imagePath, string imageLocation)
        {
            Result<string> result = new Result<string>();

            if (string.IsNullOrWhiteSpace(imageLocation))
            {
                result.SuccessResult(imagePath, "No new image selected");
                return result;
            }

            string fullPath = Path.Combine
          (AppDomain.CurrentDomain.BaseDirectory, imagePath);

            if (fullPath != imageLocation)
            {
               
                if (!string.IsNullOrEmpty(imageLocation))
                {
                    if (_CopyImageToProjectImageFolder(ref imageLocation))
                    {
                        if (!string.IsNullOrWhiteSpace(imagePath))
                        {
                            try
                            {
                                if (File.Exists(fullPath))
                                    File.Delete(fullPath);
                            }
                            catch (IOException ex)
                            {
                                result.FailureResult(imagePath, ex.Message);
                                return result;
                            }
                        }
                    }
                    else
                    {
                        result.FailureResult(imagePath, "Error: reason Not can copy image");
                        return result;
                    }

                result.SuccessResult(imageLocation, "Change Image Success");
                return result;
                }
            }
            result.SuccessResult(imagePath, "Old Image equel New Image");
            return result;
        }

    }
}
