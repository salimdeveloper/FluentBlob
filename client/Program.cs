using FluentBlob.Core;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace client
{
    class Program
    {

        static void Main(string[] args)
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;"
               + "AccountName=dpdevblobs"
               + ";AccountKey=y6JPlKpuDqNg/V46LE1P+IEjqO9OpqOWJCFgJ5dE1tW6eTYN+0fZst3n0WYGgmGFEAvTa6yLrBlGepKjE67mqg=="
               + ";EndpointSuffix=core.windows.net";
            var fileStream = System.IO.File.OpenWrite(@"C:\Users\dpdev\dnl\helloworld4.txt");
            ListAllBlobItem(storageConnectionString, "second");
            //CreateNewContainer(storageConnectionString);
            //DeleteContainer(storageConnectionString, "thrid");
            //ListAllContainers(storageConnectionString);
            // BlobService.Connect(storageConnectionString).Container("second").Upload("helloworld4.txt").FromStream(fileStream);
            // BlobService.Connect(storageConnectionString).Container("second").Download("helloworld4.txt").ToStream(fileStream);

            //BlobService.Connect(storageConnectionString).Container("third").DeleteContainer();
            //foreach (var item in _bloblist)
            //{
            //    Console.WriteLine(item.StorageUri);
            //}
            Console.ReadKey();
        }
        private static async Task ListContainersWithPrefixAsync(CloudBlobClient blobClient, string prefix)
        {
            Console.WriteLine("List all containers beginning with prefix {0}, plus container metadata:", prefix);
            
            try
            {
                ContainerResultSegment resultSegment = null;
                BlobContinuationToken continuationToken = null;

                do
                {
                    // List containers beginning with the specified prefix, returning segments of 5 results each.
                    // Passing null for the maxResults parameter returns the max number of results (up to 5000).
                    // Requesting the container's metadata with the listing operation populates the metadata,
                    // so it's not necessary to also call FetchAttributes() to read the metadata.
                    resultSegment = await blobClient.ListContainersSegmentedAsync(
                        prefix, ContainerListingDetails.Metadata, 5, continuationToken, null, null);

                    // Enumerate the containers returned.
                    foreach (var container in resultSegment.Results)
                    {
                        Console.WriteLine("\tContainer:" + container.Name);

                        // Write the container's metadata keys and values.
                        foreach (var metadataItem in container.Metadata)
                        {
                            Console.WriteLine("\t\tMetadata key: " + metadataItem.Key);
                            Console.WriteLine("\t\tMetadata value: " + metadataItem.Value);
                        }
                    }

                    // Get the continuation token. If not null, get the next segment.
                    continuationToken = resultSegment.ContinuationToken;

                } while (continuationToken != null);
            }
            catch (StorageException e)
            {
                Console.WriteLine("HTTP error code {0} : {1}",
                                    e.RequestInformation.HttpStatusCode,
                                    e.RequestInformation.ErrorCode);
                Console.WriteLine(e.Message);
            }
        }
        private static void CreateNewContainer(string connectionString)
        {
            var _result = BlobService.Connect(connectionString).Container("third").DeleteContainer(true);
            Console.WriteLine(_result.ToString());
        }
        private static void DeleteContainer(string connectionString, string containerName)
        {
            var result = BlobService.Connect(connectionString: connectionString).Container("third").DeleteContainer(false);
            Console.WriteLine("Deleting Container :" + containerName);
            Console.WriteLine(result.ToString());
            
        }
        private static void ListAllContainers(string connectionString)
        {
            var _containers = BlobService.Connect(connectionString).GetAllContainers();
            foreach (var item in _containers)
            {
                Console.WriteLine("Name :"+ item.Name +"\n" + item.StorageUri +"\n");
                Console.WriteLine("Properties : " + item.Properties.LeaseState +"\n");
                Console.WriteLine("Etag : " + item.Properties.ETag + "\n");
            }
        }
        private static void ListAllBlobItem(string connectionString, string containerName)
        {
            var _list = BlobService.Connect(connectionString).Container(containerName).GetAllBlobItems();
            foreach (var item in _list)
            {
                Console.WriteLine(item.Uri);
            }
           
        }


    }
}
