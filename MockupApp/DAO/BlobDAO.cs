using CloudinaryDotNet;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MockupApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web; 

namespace MockupApp.DAO
{
    public class BlobDAO
    {
        private readonly CloudBlobClient _blobClient;
        
        public BlobDAO()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["blob"].ConnectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        public  string UploadImage(HttpPostedFileBase contenido)
        {

          
            CloudBlobContainer container = _blobClient.GetContainerReference("mockups");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(contenido.FileName);
            blockBlob.UploadFromStream(contenido.InputStream);
            return contenido.FileName;
        }

        public void EliminarArchivo(string nombre)
        {
            CloudBlobContainer container = _blobClient.GetContainerReference("mockups");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(nombre);
            blockBlob.Delete();
        }


    }
}