using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProLayout.Editor
{
    [CustomEditor(typeof(ArrayCopy))]
    public class ArrayCopyEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset = default;

        private ArrayCopy _arrayCopy;

        private void OnEnable()
        {
            _arrayCopy = (ArrayCopy)target;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var visualTree = visualTreeAsset.CloneTree();
            // 默认Inspector项
            var inspectorFoldout = visualTree.Q("fold_DefaultInspector");
            InspectorElement.FillDefaultInspector(inspectorFoldout, serializedObject, this);

            var updateButton = visualTree.Q<Button>("btn_UpdateArray");
            updateButton.clicked += () => { _arrayCopy.Generate(); };

            return visualTree;
        }
    }
}