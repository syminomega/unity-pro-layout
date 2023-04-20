using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProLayout.Utilities
{
    public class ControlPointHandle : MonoBehaviour
    {
        [SerializeField] private ControlPoint controlPoint;
        private readonly Color _pMainColor = new Color(0.453f, 0.727f, 0.262f);

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position,
                "Packages/com.symin.pro-layout/Gizmos/cp_anchor.png",
                true, _pMainColor);
        }
    }
}