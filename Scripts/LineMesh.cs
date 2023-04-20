using ProLayout.Utilities;
using UnityEngine;

namespace ProLayout
{
    [ExecuteInEditMode]
    public class LineMesh : MonoBehaviour
    {
        #region 组件属性

        private MeshRenderer _meshRenderer;

        //生成的网格
        [SerializeField] private Mesh mesh;

        //生成的子对象
        [SerializeField] private GameObject subMeshObject;
        [SerializeField] private GameObject handleCollectionObject;

        //是否处于编辑模式
        [HideInInspector] public bool isEditMode;

        //是否显示子对象
        public bool showDetailObjects;

        #endregion


        #region 网格属性

        //控制点
        [SerializeField] public ControlPoint[] controlPoints;

        //线宽度
        [SerializeField] [Min(0)] private float lineWidth = 1f;

        //材质
        [SerializeField] private Material lineMaterial;

        #endregion

        private void Awake()
        {
            if (subMeshObject == null)
            {
                SetupObjects();
#if UNITY_EDITOR
                InitHandlers();
#endif
            }
        }

        private void OnValidate()
        {
            SetupObjects();
            InitHandlers();
        }

        //初始化所需的对象
        private void InitObjects()
        {
            //创建渲染所需的网格对象
            subMeshObject = new GameObject("lineObj");
            subMeshObject.transform.SetParent(this.transform, false);

            var meshFilter = subMeshObject.AddComponent<MeshFilter>();
            _meshRenderer = subMeshObject.AddComponent<MeshRenderer>();
            mesh = new Mesh
            {
                name = "generated line"
            };
            meshFilter.mesh = mesh;
        }

        private void SetupObjects()
        {
            if (subMeshObject == null)
            {
                InitObjects();
            }

            if (_meshRenderer == null)
            {
                _meshRenderer = subMeshObject.GetComponent<MeshRenderer>();
            }

            if (!showDetailObjects)
            {
                subMeshObject.hideFlags |= HideFlags.HideInHierarchy;
            }
            else
            {
                subMeshObject.hideFlags ^= HideFlags.HideInHierarchy;
            }

            UpdateMesh();
            _meshRenderer.material = lineMaterial;
        }


        //更新网格数据
        private void UpdateMesh()
        {
            if (controlPoints == null || controlPoints.Length < 2)
            {
                return;
            }

            mesh.Clear();
            mesh.vertices = new[]
            {
                new Vector3(0, 0, 0), new Vector3(1, 0, 0),
                new Vector3(0, 0, 1), new Vector3(1, 0, 1)
            };
            mesh.triangles = new[]
            {
                0, 1, 2,
                1, 3, 2,
            };
            mesh.uv = new[]
            {
                new Vector2(0, 0), new Vector2(1, 0),
                new Vector2(0, 1), new Vector2(1, 1),
            };
            mesh.RecalculateNormals();
        }

        private void InitHandlers()
        {
            if (handleCollectionObject == null)
            {
                //创建控制器所需的集合对象
                handleCollectionObject = new GameObject("handlesObj");
                handleCollectionObject.transform.SetParent(this.transform, false);
                handleCollectionObject.hideFlags |= HideFlags.DontSaveInBuild;
            }
        
            if (!showDetailObjects)
            {
                handleCollectionObject.hideFlags |= HideFlags.HideInHierarchy;
            }
            else
            {
                handleCollectionObject.hideFlags ^= HideFlags.HideInHierarchy;
            }
        }

        public void SetupHandles()
        {
            // 为每个控制点添加 Handle 对象
            for (var i = 0; i < controlPoints.Length; i++)
            {
                var cp = controlPoints[i];
                var cpObject = new GameObject("cp" + i);
                cpObject.transform.SetParent(handleCollectionObject.transform);
                cpObject.transform.position = transform.position + cp.pMain;
                cpObject.AddComponent<ControlPointHandle>();
            }
        }
    }
}