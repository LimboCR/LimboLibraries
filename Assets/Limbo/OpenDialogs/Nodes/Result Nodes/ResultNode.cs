using UnityEngine;

namespace Limbo.OpenDialogs
{
    public class DialogResult : ScriptableObject
    {
        public virtual void ApplyResult(GameObject target) { }
    }
}