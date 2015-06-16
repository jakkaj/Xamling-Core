using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace XamlingCore.Windows8.Implementations.Helpers
{
    /// <summary>
    /// Replaces the missing TryGetItemAsync method in Windows Phone 8.1
    /// Thanks to http://dotnet.dzone.com/articles/movedcheeseexception-%E2%80%93-where Joost Van Schaik
    /// </summary>
    public static class StorageFolderExtensions
    {
        public static async Task<IStorageItem> TryGetItemAsync(this IStorageFolder folder, string name)
        {
            var fileName = Path.GetFileName(name);
            var folderName = Path.GetDirectoryName(name);

            if (folderName != null)
            {
                folder = await folder.TryGetFolderAsync(folderName);
            }

            if (folder == null)
            {
                return null;
            }
 
            var files = await folder.GetItemsAsync();
            return files.FirstOrDefault(p => p.Name == fileName);
        }

        public static async Task<IStorageFile> TryGetFileAsync(this IStorageFolder folder, string name)
        {
            var fileName = Path.GetFileName(name);
            var folderName = Path.GetDirectoryName(name);

            if (folderName != null)
            {
                folder = await folder.TryGetFolderAsync(folderName);
            }

            if (folder == null)
            {
                return null;
            }

            var files = await folder.GetFilesAsync();
            return files.FirstOrDefault(p => p.Name == fileName);
        }

        public static async Task<IStorageFolder> TryGetFolderAsync(this IStorageFolder folder, string name)
        {
            if (name.Contains("\\"))
            {
                var s = name.Split('\\');

                if (s.Length > 1)
                {
                    for (var i = 0; i < s.Length; i++)
                    {
                        var newFolder = await _tryGetFolder(folder, s[i]);

                        if (newFolder == null)
                        {
                            return null;
                        }

                        folder = newFolder;
                    }
                }
            }
            else
            {
                var newFolder = await _tryGetFolder(folder, name);
                return newFolder;
            }

            return folder;
        }

        static async Task<IStorageFolder> _tryGetFolder(IStorageFolder parent, string name)
        {
            var files = await parent.GetFoldersAsync();
            return files.FirstOrDefault(p => p.Name == name);
        }
    }
}