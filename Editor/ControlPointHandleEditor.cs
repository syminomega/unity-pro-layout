using System;
using System.Collections;
using System.Collections.Generic;
using ProLayout.Utilities;
using UnityEditor;
using UnityEngine;

namespace ProLayout.Editor
{
    [CustomEditor(typeof(ControlPointHandle))]
    public class ControlPointHandleEditor : UnityEditor.Editor
    {
        private ControlPointHandle _controlPointHandle;
        private Quaternion _quaternionDefault;

        private void OnEnable()
        {
            _controlPointHandle = (ControlPointHandle)target;
            _quaternionDefault = Quaternion.Euler(0, 0, 0);
        }
    }
}