using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MonoTouch.Foundation;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Util.Lock;
using XamlingCore.Portable.Util.Util;

namespace XamlingCore.iOS.Implementations
{
    public class LocalStorage : ILocalStorage
    {
        public async System.Threading.Tasks.Task<bool> Copy(string source, string newName, bool replace = true)
        {
            var _lock = NamedLock.Get(newName);
            using (var releaser = await _lock.LockAsync())
            {

                var sFile = _getPath(source);
                var tFile = _getPath(newName);

                if (!File.Exists(sFile))
                {
                    return false;
                }

                File.Copy(sFile, tFile, replace);

                return true;
            }
        }

        public async System.Threading.Tasks.Task<bool> DeleteFile(string fileName)
        {
            var _lock = NamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);

                if (File.Exists(path))
                {
                    File.Delete(path);    
                }

                return true;
            }
        }

        public async System.Threading.Tasks.Task<bool> FileExists(string fileName)
        {
            var p = _getPath(fileName);
            return File.Exists(p);
        }

        public async System.Threading.Tasks.Task<List<string>> GetAllFilesInFolder(string folderPath)
        {
            var p = _getPath(folderPath);
            if (!Directory.Exists(p))
            {
                return null;
            }

            var f = Directory.GetFiles(p);

            return f != null ? f.ToList() : null;
        }
       

        public async System.Threading.Tasks.Task<byte[]> Load(string fileName)
        {
            var _lock = NamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);

                if (!File.Exists(path))
                {
                    return null;
                }

                return File.ReadAllBytes(path);
            }
        }

        public async System.Threading.Tasks.Task<System.IO.Stream> LoadStream(string fileName)
        {
            var _lock = NamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);
                if (!File.Exists(path))
                {
                    return null;
                }

                return File.Open(path, FileMode.Open);
            }
        }

        

        public async System.Threading.Tasks.Task<string> LoadString(string fileName)
        {
            var _lock = NamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);

                if (!File.Exists(path))
                {
                    return null;
                }

                return File.ReadAllText(path);
            }
        }

        public async System.Threading.Tasks.Task Save(string fileName, byte[] data)
        {
            var _lock = NamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);
                _createDirForFile(path);

                File.WriteAllBytes(path, data);
            }
        }

        public async System.Threading.Tasks.Task SaveStream(string fileName, System.IO.Stream stream)
        {
            var _lock = NamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);
            
                _createDirForFile(path);

                using (var s = File.Create(fileName))
                {
                    await stream.CopyToAsync(s);
                }
            }
        }

        public async System.Threading.Tasks.Task<bool> SaveString(string fileName, string data)
        {
            var _lock = NamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);
                _createDirForFile(path);

                File.WriteAllText(path, data);
                return true;
            }

        }

        void _createDirForFile(string fileName)
        {
            var dir = Path.GetDirectoryName(fileName);

            if (dir == null)
            {
                return;
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// iOS Specific get path
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string _getPath(string fileName)
        {
            var documents = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.LibraryDirectory, NSSearchPathDomain.User)[0];
            var str = documents.Path;
            var result = Path.Combine(str, fileName);
            return result;
        }
    }
}