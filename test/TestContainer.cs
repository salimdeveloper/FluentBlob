using Microsoft.Azure.Storage;
using System;
using Xunit;
using FluentBlob.Core;
using Microsoft.Azure.Storage.Blob;
using System.Linq;

namespace FluentBlob.Test
{
    public class TestContainer : IClassFixture<BlobStorageFixture>
    {
        private readonly string _conString = "UseDevelopmentStorage=true;";
        private BlobStorageFixture _fixture;

        public TestContainer(BlobStorageFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public void CreateContainer_ForContainerCreated_ReturnsTrue()
        {
            string _containerName = "hello";

            BlobService.Connect(_conString).Container(_containerName).CreateContainer();

            var _containerExists = Client().GetContainerReference(_containerName).Exists();

            Assert.True(_containerExists);

            //Clean up
            Client().GetContainerReference(_containerName).DeleteIfExists();
        }
        [Fact]
        public void DeleteContainer_ForContainerDeleted_ReturnsTrue()
        {
            string _containerName = "todelete";

            Client().GetContainerReference(_containerName).Create();
            BlobService.Connect(_conString).Container(_containerName).DeleteContainer(true);
            var _containerExists = Client().GetContainerReference(_containerName).Exists();

            Assert.False(_containerExists);
        }
        [Fact]
        public void GetAllContainers_ForAllContainers_ReturnsCount()
        {
            string _containerName = "container";
            int _noofContainers = 5;
            for (int i = 0; i < _noofContainers; i++)
            {
                Client().GetContainerReference(_containerName+"-"+i).Create();
            }
            var _result = BlobService.Connect(_conString).GetAllContainers().ToList().Count;

            Assert.Equal(_noofContainers, _result);
        }

        static CloudBlobClient Client() => CloudStorageAccount.Parse("UseDevelopmentStorage=true;").CreateCloudBlobClient();
    }
}
