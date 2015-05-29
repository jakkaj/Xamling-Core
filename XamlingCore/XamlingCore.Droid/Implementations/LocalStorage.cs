using Android.App;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Util.Lock;


namespace XamlingCore.Droid.Implementations
{
    public class LocalStorage : ILocalStorage
    {
        public char Separator()
        {
            return Path.DirectorySeparatorChar;
        }

        public async Task<string> GetFullPath(string fileName)
        {
            return _getPath(fileName);
        }

        public async Task<bool> IsZero(string fileName)
        {
            var _lock = XNamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);

                var r = File.Exists(path);


                if (!r)
                {
                    return false;
                }

                var b = await Load(fileName);

                return b.Length == 0;
            }
        }

        public async Task<bool> Copy(string source, string newName, bool replace = true)
        {
            var _lock = XNamedLock.Get(newName);
            using (var releaser = await _lock.LockAsync())
            {

                var sFile = _getPath(source);
                var tFile = _getPath(newName);

                var r = File.Exists(sFile);

                if (!r)
                {
                    return false;
                }

                var dir = Path.GetDirectoryName(tFile);

                if (dir != null && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.Copy(sFile, tFile, replace);

                return true;
            }
        }

        public async Task<bool> DeleteFile(string fileName)
        {
            var _lock = XNamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);

                var r = File.Exists(path);

                if (r)
                {
                    File.Delete(path);
                }

                return true;
            }
        }

        public async Task<bool> FileExists(string fileName)
        {
            var p = _getPath(fileName);

            var r = File.Exists(p);

            return r;
        }

        public async Task<List<string>> GetAllFilesInFolder(string folderPath, bool recurse)
        {
            var p = _getPath(folderPath);
            if (!Directory.Exists(p))
            {
                return null;
            }

            var f = Directory.GetFiles(p, "*.*", recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

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

                var r = File.ReadAllBytes(path);

                return r;
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

                var r = File.Open(path, FileMode.Open);
                return r;
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

                var r = File.ReadAllText(path);

                return r;
            }
        }

        public async Task Save(string fileName, byte[] data)
        {
            var _lock = XNamedLock.Get(fileName);

            using (var releaser = await _lock.LockAsync())
            {
                var path = _getPath(fileName);
                _createDirForFile(path);

                File.WriteAllBytes(path, data);
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

        static XAsyncLock wl = new XAsyncLock();
        public async Task<bool> SaveString(string fileName, string data)
        {
            var _lock = XNamedLock.Get(fileName);

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

        private string _getPath(string fileName)
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);            
        }
    }    
}

