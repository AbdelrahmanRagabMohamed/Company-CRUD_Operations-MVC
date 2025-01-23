using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;


namespace Demo_03_.PL.Helpers
{
    public class DocumentSettings
    {
        // Upload File
        public static string UploadFile(IFormFile file, string folderName)
        // Returns String => (FileName) THat it Will be Stored inDB
        {

            // 1. Get Located Folder Path 
            /// string FolderPath = "C:\\Users\\xps store\\OneDrive\\Desktop\\Demo_05 MVC Project\\Demo_03 .PL\\wwwroot\\Files\\Images\\";
            ///  string FolderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\" + folderName;
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

            // 2. Get File Name and Make it Unique 
            string FileName = ($"{Guid.NewGuid()}{file?.FileName}");        

            // 3. Get File Path[Folder Path + FileName]
            string FilePath = Path.Combine(FolderPath, FileName);

            // 4. Save File As Streams
            using var FS = new FileStream(FilePath, FileMode.Create);
            file?.CopyTo(FS);

            // 5. Return File Name
            return FileName;
        }


        // Delete File
        public static void DeleteFile(string FileName, string FolderName)
        {
            // Get File Path
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//Files", FolderName, FileName);

            // Check if File is Exists or Not : If Exists Remove it if not Exists Do Nothing

            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }



        }
    }
}
