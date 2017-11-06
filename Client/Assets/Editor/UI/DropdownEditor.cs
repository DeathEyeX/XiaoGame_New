using UnityEditor;
using UnityEngine.UI;

namespace Hotfire.UI
{
    [CustomEditor(typeof(Dropdown), true)]
    [CanEditMultipleObjects]
    public class DropdownEditor : UnityEditor.UI.DropdownEditor
    {
    }
}
