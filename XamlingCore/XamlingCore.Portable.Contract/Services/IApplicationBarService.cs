using System;

namespace XamlingCore.Portable.Contract.Services
{
    public interface IApplicationBarService
    {
        void Add(string name, Action callback = null, string insertBefore = null);

        /// <summary>
        /// Called during navigation events
        /// </summary>
        void Reset();

        void AddConfiguration(string name, string text, bool isButton = true, string imageUri = null, Action defaultCallback = null, bool isDefault = false);
        void Clear();
        void Hide();
        void GoMini();
        void SetColor(string foreground, string background);
        void AddSkip(string name);
        void Show();
    }
}