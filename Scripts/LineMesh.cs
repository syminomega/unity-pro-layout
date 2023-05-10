using ProLayout.Utilities;
using System.Collections.Generic;
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
        [SerializeField][Min(0)] private float lineWidth = 1f;

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
            //存放顶点
            List<Vector3> meshVertices = new List<Vector3>();
            //存放UV
            List<Vector2> meshUVs = new List<Vector2>();
            //存放三角形顶点索引
            List<int> meshTriangles = new List<int>();
            //累加长度
            float totalLength = 0;
            //网格分段索引，用于计算三角形顶点索引
            int segIndex = 0;
            //上一个点位置
            Vector3 lastPoint = controlPoints[0].pMain;

            //计算初始顶点信息
            var p0L = BezierUtility.GetBezierOffsetPoint(controlPoints[0], controlPoints[1], 0, lineWidth / 2);
            var p0R = BezierUtility.GetBezierOffsetPoint(controlPoints[0], controlPoints[1], 0, -lineWidth / 2);

            meshVertices.Add(p0L);
            meshVertices.Add(p0R);
            meshUVs.Add(new Vector2(0, 0));
            meshUVs.Add(new Vector2(0, 1));


            //循环每个控制点，从第二个点开始，
            for (var pI = 1; pI < controlPoints.Length; pI++)
            {
                //按step分割t
                for (var t = 0.2f; t <= 1; t += 0.2f)
                {
                    //计算当前t的两侧顶点
                    var pL = BezierUtility.GetBezierOffsetPoint(controlPoints[pI - 1], controlPoints[pI], t, lineWidth / 2);
                    var pR = BezierUtility.GetBezierOffsetPoint(controlPoints[pI - 1], controlPoints[pI], t, -lineWidth / 2);
                    //添加两侧顶点
                    meshVertices.Add(pL);
                    meshVertices.Add(pR);
                    //获取当前中心点的位置
                    var currentPoint = BezierUtility.GetBezierPoint(controlPoints[pI - 1], controlPoints[pI], t);
                    //计算当前点到上一个点的距离，并计算总长度
                    var distance = Vector3.Distance(currentPoint, lastPoint);
                    totalLength += distance;
                    lastPoint = currentPoint;
                    //计算当前顶点的uv坐标
                    var uvL = new Vector2(totalLength / lineWidth, 0);
                    var uvR = new Vector2(totalLength / lineWidth, 1);
                    //添加uv
                    meshUVs.Add(uvL);
                    meshUVs.Add(uvR);
                    //添加三角形索引
                    meshTriangles.Add(segIndex * 2);
                    meshTriangles.Add(segIndex * 2 + 1);
                    meshTriangles.Add(segIndex * 2 + 2);
                    meshTriangles.Add(segIndex * 2 + 1);
                    meshTriangles.Add(segIndex * 2 + 3);
                    meshTriangles.Add(segIndex * 2 + 2);

                    segIndex++;
                }
            }
            mesh.vertices = meshVertices.ToArray();
            mesh.uv = meshUVs.ToArray();
            mesh.triangles = meshTriangles.ToArray();
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