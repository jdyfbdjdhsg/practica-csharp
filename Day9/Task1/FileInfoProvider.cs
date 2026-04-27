using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    public class FileInfoProvider
    {
        public (long Size, DateTime CreationTime, DateTime LastModified) GetFileInfo(string path)
        {
            var info = new FileInfo(path);
            return (info.Length, info.CreationTime, info.LastWriteTime);
        }

        public (bool CanRead, bool CanWrite, bool CanExecute) CheckPermissions(string path)
        {
            var info = new FileInfo(path);
            return (!info.IsReadOnly, info.IsReadOnly, false);
        }
    }
}
