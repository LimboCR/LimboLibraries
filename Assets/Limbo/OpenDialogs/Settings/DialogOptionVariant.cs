using System;
using UnityEngine;


namespace Limbo.OpenDialogs
{
    [Serializable]
    public class DialogOptionVariant
    {
        [TextArea] public string OptionText;
        public DialogNode NextNode;
        public DialogResult Result;
    }
}