using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Data.Entities
{
    public class KeyEntityBase
    {
        private Func<string, string> _keyModifierCallback;
        public void SetKeyModifier(Func<string, string> keyModifierCallback)
        {
            _keyModifierCallback = keyModifierCallback;
        }

        public virtual string GetKey(string key)
        {
            return _keyModifierCallback != null ? _keyModifierCallback(key) : key;
        }
    }
}
