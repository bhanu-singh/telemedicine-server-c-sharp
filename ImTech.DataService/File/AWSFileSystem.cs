using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.Model;
using Amazon;
using Amazon.S3.Model;
using Amazon.S3.IO;
using System.IO;

namespace ImTech.DataServices.File
{
    public class AWSFileSystem : FileSystem
    {
        string bucketName;
        IAmazonS3 s3Client;


        public AWSFileSystem()
        {
            bucketName = ConfigurationManager.AWSBucketName;
            s3Client = new AmazonS3Client(ConfigurationManager.AWSAccessKey, ConfigurationManager.AWSSecrateAccessKey, GetRegionEndPoint());
        }
        public override bool Delete(NonSecureFileModel fileModel)
        {
            DeleteObjectRequest request = new DeleteObjectRequest()
            {
                BucketName = bucketName,
                Key = fileModel.FileFullPath + fileModel.FileName
            };

            s3Client.DeleteObject(request);
            return true;
        }

        public override string GetFolderPath(NonSecureFileModel filemodel)
        {
            string folder = CreateAbsoultePath(filemodel.FileDocType);
            S3DirectoryInfo dirInfo = new S3DirectoryInfo(s3Client, bucketName, folder);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            return folder;
        }
        public override NonSecureFileModel Read(NonSecureFileModel fileModel)
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = fileModel.FileFullPath + fileModel.FileName
            };
            //request.ServerSideEncryptionCustomerProvidedKey= ServerSideEncryptionMethod.AES256;
            try
            {
                using (GetObjectResponse response = s3Client.GetObject(request))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        response.ResponseStream.CopyTo(memoryStream);
                        fileModel.Contents = memoryStream.ToArray();
                    }
                }
            }
            catch (Exception e)
            { }

            return fileModel;
        }

        public override bool Write(NonSecureFileModel fileModel)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = fileModel.ContentStream,
                BucketName = bucketName,
                Key = fileModel.FileFullPath + fileModel.FileName
                
            };
            //request.ServerSideEncryptionMethod=ServerSideEncryptionMethod.AES256;
            PutObjectResponse response = s3Client.PutObject(request);
            fileModel.ContentStream.Dispose();
            fileModel.ContentStream = null;
            return true;
        }

        internal override string CreateAbsoultePath(string docType)
        {
            return docType + ConfigurationManager.FileSaparator;
        }

        private RegionEndpoint GetRegionEndPoint()
        {
            return RegionEndpoint.GetBySystemName(ConfigurationManager.AWSRegion);
        }

    }
}
