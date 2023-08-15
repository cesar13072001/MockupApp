using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace MockupApp.DAO
{
    public class CloudinaryDAO
    {
        static Account account = new Account(
            ConfigurationManager.AppSettings["cloudCloudinary"],
            ConfigurationManager.AppSettings["apiKeyCloudinary"],
            ConfigurationManager.AppSettings["apiSecretCloudinary"]);

        Cloudinary cloudinary = new Cloudinary(account);
   
        public  string[] guardarContenido(HttpPostedFileBase contenido)
        {
            string[] respuesta = new string[2];    
            try
            {            
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(contenido.FileName, contenido.InputStream),  
                    Folder = "mockups",
                };
                var uploadResult = cloudinary.Upload(uploadParams);
                respuesta[0] = (uploadResult.AssetId.ToString());
                respuesta[1] = (uploadResult.SecureUrl.AbsoluteUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = null;
            }

            return respuesta;
        }
    }
}