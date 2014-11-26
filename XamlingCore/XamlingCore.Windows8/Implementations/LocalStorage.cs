using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Util.Lock;

namespace XamlingCore.Windows8.Implementations
{
    public class LocalStorageWindows8 : ILocalStorage
    {
        public async Task<bool> IsZero(string fileName)
        {
            var _lock = NamedLock.Get(fileName);
            using (var releaser = await _lock.LockAsync())
            {
                var file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

                using (var s = await file.OpenStreamForReadAsync())
                {
                    return s.Length == 0;
                }
            }
        }

        public async Task<List<string>> GetAllFilesInFolder(string folderPath)
        {

            var path = folderPath.Split('\\').ToList();

            if (path.Count == 0)
            {
                return null;
            }

            var currentFolder = ApplicationData.Current.LocalFolder;

            while (path.Count > 0)
            {
                var p = path.FirstOrDefault();
                path.RemoveAt(0);

                currentFolder = await currentFolder.GetFolderAsync(p);
            }

            var files = await currentFolder.GetFilesAsync();

            return files.Select(_ => _.Name).ToList();
        }

        public async Task<bool> EnsureFolderExists(string folderPath)
        {
            var _lock =  NamedLock.Get(folderPath);
            using (var l = await _lock.LockAsync())
            {
                var path = folderPath.Split('\\').ToList();

                if (path.Count == 0)
                {
                    return false;
                }

                return await _createFolder(path, Windows.Storage.ApplicationData.Current.LocalFolder);
            }

        }

        async Task<bool> _createFolder(List<string> path, StorageFolder currentFolder)
        {
            if (path.Count == 0)
            {
                return false;
            }

            var current = path.FirstOrDefault();

            if (current == null)
            {
                return false;
            }

            path.RemoveAt(0);


            try
            {
                if (!await _folderExists(current, currentFolder))
                {
                    await currentFolder.CreateFolderAsync(current);
                }
            }
            catch { }


            if (path.Count == 0)
            {
                return await _folderExists(current, currentFolder);
            }

            return await _createFolder(path, await currentFolder.GetFolderAsync(current));
        }

        async Task<bool> _folderExists(string name, StorageFolder currentFolder)
        {
            var f = await currentFolder.GetFoldersAsync();
            return (f.FirstOrDefault(_ => _.Name == name) != null);
        }

        public async Task<bool> FileExists(string fileName)
        {
            Debug.WriteLine("Checking file: {0}", fileName);
            try
            {

                var file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

                if (file != null)
                {
                    return true;
                }
            }
            catch (FileNotFoundException ex)
            {

            }
            return false;
        }

        public Task<bool> Copy(string source, string newName, bool replace = true)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Copy(string source, string destinationFolder, string newName, bool replace = true)
        {
            var _lock =  NamedLock.Get(destinationFolder + "\\" + newName);
            using (var releaser = await _lock.LockAsync())
            {
                if (!await FileExists(source)) return false;

                var file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(source);

                var dFolder =
                    await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync(destinationFolder);

                var result =
                    await
                        file.CopyAsync(dFolder, newName,
                            replace ? NameCollisionOption.ReplaceExisting : NameCollisionOption.FailIfExists);

                return result != null;
            }
        }

        public async Task<string> LoadString(string fileName)
        {
            var _lock =  NamedLock.Get(fileName);
            using (var releaser = await _lock.LockAsync())
            {
                if (!await FileExists(fileName)) return null;

                var file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

                using (var s = await file.OpenStreamForReadAsync())
                {
                    var data = new byte[s.Length];
                    await s.ReadAsync(data, 0, (int)s.Length);
                    return GetString(data);
                }
            }
        }

        public async Task<byte[]> Load(string fileName)
        {
            var _lock =  NamedLock.Get(fileName);
            using (var releaser = await _lock.LockAsync())
            {
                Debug.WriteLine("Reading file: {0}", fileName);
                if (!await FileExists(fileName)) return null;

                var file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

                using (var s = await file.OpenStreamForReadAsync())
                {
                    var data = new byte[s.Length];
                    await s.ReadAsync(data, 0, (int)s.Length);
                    return data;
                }
            }
        }

        public async Task<Stream> LoadStream(string fileName)
        {
            var _lock =  NamedLock.Get(fileName);
            using (var releaser = await _lock.LockAsync())
            {
                Debug.WriteLine("Reading file: {0}", fileName);
                if (!await FileExists(fileName)) return null;

                var file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

                return await file.OpenStreamForReadAsync();
            }
        }

        public async Task<bool> SaveString(string fileName, string data)
        {
            var _lock =  NamedLock.Get(fileName);
            using (var releaser = await _lock.LockAsync())
            {
                try
                {
                    var dataBytes = GetBytes(data);
                    var file =
                        await
                            Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(fileName,
                                CreationCollisionOption.ReplaceExisting);
                    using (var s = await file.OpenStreamForWriteAsync())
                    {
                        await s.WriteAsync(dataBytes, 0, dataBytes.Length);
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        public async Task Save(string fileName, byte[] data)
        {
            var _lock =  NamedLock.Get(fileName);
            using (var releaser = await _lock.LockAsync())
            {
                Debug.WriteLine("Writing file: {0}", fileName);
                try
                {
                    var file =
                        await
                            Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(fileName,
                                CreationCollisionOption.ReplaceExisting);
                    using (var s = await file.OpenStreamForWriteAsync())
                    {
                        await s.WriteAsync(data, 0, data.Length);
                    }
                }
                catch
                {
                }
            }
        }

        public async Task SaveStream(string fileName, Stream stream)
        {
            var _lock =  NamedLock.Get(fileName);
            using (var releaser = await _lock.LockAsync())
            {
                Debug.WriteLine("Writing file: {0}", fileName);

                try
                {
                    var file =
                        await
                            Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(fileName,
                                CreationCollisionOption.ReplaceExisting);

                    using (var s = await file.OpenStreamForWriteAsync())
                    {
                        await stream.CopyToAsync(s);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteFile(string fileName)
        {
            var _lock =  NamedLock.Get(fileName);
            using (var releaser = await _lock.LockAsync())
            {
                try
                {
                    var file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

                    if (file != null)
                    {
                        await file.DeleteAsync();
                        return true;
                    }
                }
                catch (FileNotFoundException ex)
                {

                }
                return false;
            }
        }

        static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
