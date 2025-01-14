//
// Copyright (c) 2010-2022 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
// 

using System.IO;
using System.Collections.Generic;

namespace Antmicro.Renode.Utilities
{
    public class SimpleFileCache
    {
        public SimpleFileCache(string location)
        {
            cacheLocation = Path.Combine(Emulator.UserDirectoryPath, location);

            internalCache = new HashSet<string>();
            Populate();
        }
        
        public bool ContainsEntryWithSha(string sha)
        {
            return !Emulator.InCIMode && internalCache.Contains(sha);
        }

        public bool TryGetEntryWithSha(string sha, out string filename)
        {
            if(Emulator.InCIMode || !ContainsEntryWithSha(sha))
            {
                filename = null;
                return false;
            }

            filename = Path.Combine(cacheLocation, sha);
            return true;
        }

        public void StoreEntryWithSha(string sha, string filename)
        {
            if(Emulator.InCIMode || ContainsEntryWithSha(sha))
            {
                return;
            }

            EnsureCacheDirectory();
            using(var locker = new FileLocker(Path.Combine(cacheLocation, lockFileName)))
            {
                FileCopier.Copy(filename, Path.Combine(cacheLocation, sha), true);
                internalCache.Add(sha);
            }
        }

        private void Populate()
        {
            EnsureCacheDirectory();
            using(var locker = new FileLocker(Path.Combine(cacheLocation, lockFileName)))
            {
                var dinfo = new DirectoryInfo(cacheLocation);
                foreach(var file in dinfo.EnumerateFiles())
                {
                    internalCache.Add(file.Name);
                }
            }
        }

        private void EnsureCacheDirectory()
        {
            Directory.CreateDirectory(cacheLocation);
        }

        private readonly HashSet<string> internalCache;
        private readonly string cacheLocation;

        private const string lockFileName = ".lock";
    }
}
