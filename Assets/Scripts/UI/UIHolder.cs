using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUI
{
    public class UIHolder : MonoBehaviour
    {
        public static UIHolder Instance;
        public static List<UI> UIs;

        public void Init()
        {
            Instance = this;
            UIs = new List<UI>();
        }

        public static UI AddElement(UI ui)
        {
            ui = Instantiate(ui, Instance.transform);
            UIs.Add(ui);

            ui.OnClosed += RemoveElement;
            return ui;
        }

        public static void RemoveElement(UI ui)
        {
            if (!UIs.Contains(ui)) return;

            ui.OnClosed -= RemoveElement;
            UIs.Remove(ui);
            Destroy(ui);
        }
    }
}