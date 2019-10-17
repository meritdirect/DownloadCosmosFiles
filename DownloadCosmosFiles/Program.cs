using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VcClient;

namespace DownloadCosmosFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseDiskPath = @"C:\Temp\";
            var baseCosmosPath = "https://cosmos15.osdinfra.net/cosmos/dsa.email.segmentation/local/users/MeritDirect/Product/";

            foreach (var streamPath in GetStreamsRecurse(baseCosmosPath, new Regex(@"\.txt$")))
            //foreach (var streamPath in GetStreamsRecurse(baseCosmosPath, new Regex(@"OneStore_Order_VID_2019.*\.txt$")))
            {
                //var relativeStreamPath = streamPath.Replace(baseCosmosPath, string.Empty);
                //var fullCosmosPath = Path.Combine(baseCosmosPath, relativeStreamPath);
                var uri = new Uri(streamPath);
                var relativeStreamPath = uri.Segments[uri.Segments.Length - 1];
                /*
                 var fullDiskPath = Path.Combine(baseDiskPath, relativeStreamPath);

                 var directory = Path.GetDirectoryName(fullDiskPath);
                 if (!Directory.Exists(directory))
                 {
                     Directory.CreateDirectory(directory);
                 }
                 */
                VC.Download(streamPath, baseDiskPath + relativeStreamPath, true, DownloadMode.OverWrite);
            }
        }

        static IEnumerable<string> GetStreamsRecurse(string baseDirectory, Regex regex)
        {
            foreach (var streamInfo in VC.GetDirectoryInfo(baseDirectory, true))
            {
                if (streamInfo.IsDirectory)
                {
                    foreach (var subStream in GetStreamsRecurse(streamInfo.StreamName, regex))
                    {
                        yield return subStream;
                    }
                }
                else if (regex.IsMatch(streamInfo.StreamName))
                {
                    yield return streamInfo.StreamName;
                }
            }
        }
    }
}

