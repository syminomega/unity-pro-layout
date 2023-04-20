using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace ProLayout.Editor
{
    [CustomEditor(typeof(LineMesh))]
    public class LineMeshEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset = default;

        private LineMesh _lineMesh;

        private void OnEnable()
        {
            _lineMesh = (LineMesh)target;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var visualTree = visualTreeAsset.CloneTree();
            // 默认Inspector项
            var inspectorFoldout = visualTree.Q("fold_DefaultInspector");
            InspectorElement.FillDefaultInspector(inspectorFoldout, serializedObject, this);

            var editButton = visualTree.Q<Button>("btn_EditSwitch");
            editButton.text = _lineMesh.isEditMode ? "Exit Edit Mode" : "Enter Edit Mode";

            editButton.clicked += () =>
            {
                if (_lineMesh.isEditMode)
                {
                    ExitEditMode();
                    editButton.text = "Enter Edit Mode";
                    _lineMesh.isEditMode = false;
                }
                else
                {
                    EnterEditMode();
                    editButton.text = "Exit Edit Mode";
                    _lineMesh.isEditMode = true;
                }
            };

            // Return the finished inspector UI
            return visualTree;
        }

        // 进入编辑模式，创建控制点
        private void EnterEditMode()
        {
            _lineMesh.SetupHandles();
        }

        // 退出编辑模式
        private void ExitEditMode()
        {
            
        }


    }
}