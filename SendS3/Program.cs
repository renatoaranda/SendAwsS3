using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AWSUtils;
using System.Configuration;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

using Amazon.Runtime;
using Amazon.S3.Transfer;


namespace SendS3
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string fileName = args[0].ToString();

                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"{0}\{1}", ConfigurationManager.AppSettings["FolderFile"], fileName);

                Console.WriteLine("Deseja efetuar o upload do arquivo ' " + str.ToString() + " ' ? (s/n)");
                ConsoleKeyInfo info = Console.ReadKey();
                Console.WriteLine("");
                if (info.KeyChar == 's' || info.KeyChar == 'S')
                {
                    Console.WriteLine("Aguarde...");
                    SendToS3(str.ToString(), fileName);
                    Console.WriteLine("Upload realizado");
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar arquivo para o S3 : " + ex);
            }           
        }

        private static void SendToS3(string localFilePath, string fileNameInS3)
        {
            AWSCredentials credentials = new BasicAWSCredentials(ConfigurationManager.AppSettings["AWSAccessKey"], ConfigurationManager.AppSettings["AWSSecretKey"]);
            AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = "s3.amazonaws.com";
            config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;
            AmazonS3Client Client = new AmazonS3Client(credentials, config);

            TransferUtility utility = new TransferUtility(Client);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
            request.BucketName = ConfigurationManager.AppSettings["AWSBucketName"];
            request.Key = fileNameInS3;
            request.FilePath = localFilePath;
            utility.Upload(request);



        }



    }
}
