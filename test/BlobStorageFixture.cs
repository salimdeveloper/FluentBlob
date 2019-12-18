using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace test
{
    public class BlobStorageFixture : IDisposable
    {
        readonly Process _process;
        public BlobStorageFixture()
        {
            _process = new Process
            {
                StartInfo =
                {
                    UseShellExecute =false,
                    FileName = @"C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe",

                }
            };

            StartAndWaitForExit("stop");
            StartAndWaitForExit("clear all");
            StartAndWaitForExit("start");
        }
        public void Dispose()
        {
            StartAndWaitForExit("stop");
        }
        void StartAndWaitForExit(string arguments)
        {
            _process.StartInfo.Arguments = arguments;
            _process.Start();
            _process.WaitForExit(10000);
        }
    }
}
