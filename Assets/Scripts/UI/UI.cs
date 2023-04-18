using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUI
{
    public class UI : MonoBehaviour
    {
        public System.Action<UI> OnClosed;

        public virtual void RemoveUI()
        {
            OnClosed?.Invoke(this);
            Destroy(this);
        }
    }
}