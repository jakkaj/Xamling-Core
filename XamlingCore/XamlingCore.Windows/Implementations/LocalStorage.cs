using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Util.Lock;

namespace XamlingCore.Windows.Implementations
{
    public class LocalStorage : ILocalStorage
    {
        public async Task<bool> IsZero(string fileName)
        {
            var _lock = XNamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);

                var r = Task.Run(() => File.Exists(path));
                await r;

                if (!r.Result)
                {
                    return false;
                }

                var b = await Load(fileName);

                return b.Length == 0;
            }
        }

        public string GetFullPath(string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Copy(string source, string newName, bool replace = true)
        {
            var _lock = XNamedLock.Get(newName);
            using (var releaser = await _lock.LockAsync())
            {

                var sFile = _getPath(source);
                var tFile = _getPath(newName);

                var r = Task.Run(() => File.Exists(sFile));
                await r;

                if (!r.Result)
                {
                    return false;
                }

                var dir = Path.GetDirectoryName(tFile);

                if (dir != null && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var r2 = Task.Run(() => File.Copy(sFile, tFile, replace));

                await r2;

                return true;
            }
        }

        public async Task<bool> DeleteFile(string fileName)
        {
            var _lock = XNamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);

                var r = Task.Run(() => File.Exists(path));
                await r;

                if (r.Result)
                {
                    var r2 = Task.Run(() => File.Delete(path));
                    await r2;
                }

                return true;
            }
        }

        public async Task<bool> FileExists(string fileName)
        {
            var p = _getPath(fileName);

            var r = Task.Run(() => File.Exists(p));
            await r;
            return r.Result;
        }

        public async Task<List<string>> GetAllFilesInFolder(string folderPath, bool recurse)
        {
            var p = _getPath(folderPath);
            if (!Directory.Exists(p))
            {
                return null;
            }

            var f = Directory.GetFiles(p);

            return f != null ? f.ToList() : null;
        }


        public async Task<byte[]> Load(string fileName)
        {
            var _lock = XNamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);

                if (!File.Exists(path))
                {
                    return null;
                }

                var r = Task.Run(() => File.ReadAllBytes(path));
                await r;
                return r.Result;
            }
        }

        public async Task<System.IO.Stream> LoadStream(string fileName)
        {
            var _lock = XNamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);
                if (!File.Exists(path))
                {
                    return null;
                }

                var r = Task.Run(() => File.Open(path, FileMode.Open));
                await r;
                return r.Result;
            }
        }



        public async Task<string> LoadString(string fileName)
        {
            var _lock = XNamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);

                if (!File.Exists(path))
                {
                    return null;
                }

                var r = Task.Run(() => File.ReadAllText(path));
                await r;
                return r.Result;
            }
        }

        public async Task Save(string fileName, byte[] data)
        {
            var _lock = XNamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);
                _createDirForFile(path);

                var r = Task.Run(() => File.WriteAllBytes(path, data));
                await r;
            }
        }

        public async Task SaveStream(string fileName, System.IO.Stream stream)
        {
            var _lock = XNamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);

                _createDirForFile(path);

                using (var s = File.Create(path))
                {
                    await stream.CopyToAsync(s);
                }
            }
        }

        public async Task<bool> SaveString(string fileName, string data)
        {
            var _lock = XNamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);
                _createDirForFile(path);

                var r = Task.Run(() => File.WriteAllText(path, data));
                await r;
                return true;
            }

        }

        void _createDirForFile(string fileName)
        {
            var dir = Path.GetDirectoryName(fileName);

            //Debug.WriteLine("Creating Directory: {0}", fileName);

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
        /// iOS 8 Specific get path
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string _getPath(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
        }
    }
}