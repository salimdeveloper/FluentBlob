# FluentBlob [![NuGet Badge](https://buildstats.info/nuget/FluentBlob)](https://www.nuget.org/packages/FluentBlob/)
__FluentBlob is an API that provides a simple fluent interface to access features of Microsoft Azure Storage Blob services.__

To install, use the Nuget "FluentBlob":
```
PM> Install-Package FluentBlob -Version 1.0.0
```
__To create a new container :__
```csharp
// Use the following connection string for Storage Account Emulator.
// String storageConnectionString ="UseDevelopmentStorage=true;"
//Use the following connection string for Live Storage Account.
  string storageConnectionString = "DefaultEndpointsProtocol=https;"
                                   + "AccountName=yourblobs"
               + ";AccountKey=y6JPlKg/V46LE1P+IEjqO9Opq+0WYGgmGFEAvTa6yLrBlGepKjE67mqg=="
               + ";EndpointSuffix=core.windows.net";

  Console.WriteLine("Creating a new container ..");
  var _result = false;
  try
  {
      _result = BlobService.Connect(storageConnectionString).Container("newcontainer3").CreateContainer();
  }
  catch (Exception ex)
  {
      Console.WriteLine("Exception thrown: " + ex.ToString());
  }
  finally
  {
       string message;
       message = _result ? "Container sucessfully created!" : "Failed to create container";
       Console.WriteLine(message);
  }
 ```
 __To delete a container :__
 ```csharp
  Console.WriteLine("Deleting container ..");
  var _result = false;
  try
  {
      _result = BlobService.Connect("UseDevelopmentStorage=true;").Container("newcontainer2").DeleteContainer(false);
  }
  catch (Exception ex)
  {
       Console.WriteLine("Exception thrown: " + ex.ToString());
  }
  finally
  {
       string message;
       message = _result ? "Container sucessfully deleted!" : "Failed to delete container";
       Console.WriteLine(message);
  }
```
__List all containers in storage account :__
```csharp
Console.WriteLine("Retrieving all containers...");
try
{
   var _containerslist = BlobService.Connect("UseDevelopmentStorage=true;").GetAllContainers();
   foreach (var container in _containerslist)
   {
       Console.WriteLine(container.Name);
   }
}
catch(Exception ex)
{
   Console.WriteLine("Exception thrown: "+ex.ToString());
}
```
__Upload blob to container :__
```csharp
Console.WriteLine("uploading blob....");
try
{
    using (Stream file = File.OpenRead(@"C:\Users\DpDev\Pictures\test.png"))
    {
                        BlobService.Connect("UseDevelopmentStorage=true;").Container("newcontainer").UploadBlob("test.png").FromStream(file);
    }
    Console.WriteLine("file uploded successfully");
}
catch(Exception ex)
{
    Console.WriteLine("file upload failed with exception :" +ex.ToString());
}
```
__Download blob from container :__
```csharp
string _blobName = "test"+ ".png"; 
var container = "newcontainer";
//Prepare File
string _localPath = "./";

string _localFilePath = Path.Combine(_localPath, _blobName);
Console.WriteLine("Downloading Blob....");
try
{
    using FileStream _filestream = File.OpenWrite(_localFilePath);
                  BlobService.Connect("UseDevelopmentStorage=true;").Container(container).DownloadBlob(_blobName).ToStream(_filestream);
    Console.WriteLine("Blob Downloaded successfuly.");
}
catch(Exception ex)
{
    Console.WriteLine("File download failed :"+ex.ToString());
}
```
__Delete blob from container :__
```csharp
string _containerName = "newcontainer";
string _blobName = "test" + ".png";
Console.WriteLine("Deleting blob ..");
var _result = false;
try
{
    _result= BlobService.Connect("UseDevelopmentStorage=true;").Container(_containerName).DeleteBlob(_blobName);
}
catch (Exception ex)
{
    Console.WriteLine("Exception thrown: " + ex.ToString());
}
finally
{
    string message;
    message = _result ? "Blob deleted successfuly!" : "Blob deletion failed";
    Console.WriteLine(message);
}
```
__List all blob items in a container :__
```csharp
 Console.WriteLine("Getting all blobs from container...");
 try
 {
 var _blobitems = BlobService.Connect("UseDevelopmentStorage=true;").Container("newcontainer").GetAllBlobItems();
    foreach (var blob in _blobitems)
    {
        Console.WriteLine(blob.Uri+"\n");
    }
 }
  catch (Exception ex)
 {

     Console.WriteLine("Exception thrown: " + ex.ToString());
 }
```
__Get shared access Url of the blob :__
  ```
  Shared access url of a blob enables to open in browser
  ```
```charp
Console.WriteLine("getting The Shared Access Url...");
try
{
    string _blobName = "test.png";
    string _containerName = "newcontainer";
    int _minute_The_Url_be_Live = 10; //The shared access will be live for 10 minutes.
    var _sharedAccessUri = BlobService.Connect("UseDevelopmentStorage=true;").Container(_containerName)
                           .GetSharedUri(_blobName, _minute_The_Url_be_Live);
    Console.WriteLine("The Uri With Shared Access for" + _blobName + "is :" + _sharedAccessUri);
}
catch (Exception ex)
{
    Console.WriteLine("Exception has occured :" + ex.ToString());
}
```

                    
