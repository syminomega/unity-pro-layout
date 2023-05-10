using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ProLayout
{
    public class ArrayCopy : MonoBehaviour
    {
        // 需要拷贝的对象
        public GameObject prefabToCopy;

        // 存放拷贝对象的集合
        [SerializeField] private GameObject copiedCollectionObject;

        public float distanceX;
        [Min(1)] public int countX = 1;

        public float distanceY;
        [Min(1)] public int countY = 1;

        public float distanceZ;
        [Min(1)] public int countZ = 1;

        public bool useContainerRotation = true;
        public void Generate()
        {
            //删除原有模型
            if (copiedCollectionObject != null)
            {
                DestroyImmediate(copiedCollectionObject);
                copiedCollectionObject = null;
            }

            //判断当前是否指定模型
            if (prefabToCopy == null)
            {
                Debug.LogWarning("没有指定要阵列复制的对象");
                return;
            }

            //创建阵列集合对象
            copiedCollectionObject = new GameObject("copiedCollectionObj");
            copiedCollectionObject.transform.SetParent(this.transform, false);
            copiedCollectionObject.hideFlags |= HideFlags.HideInHierarchy;

            //使用linq生成xyz的组合
            var xyz =
                from x in Enumerable.Range(0, countX)
                from y in Enumerable.Range(0, countY)
                from z in Enumerable.Range(0, countZ)
                select new { x, y, z };

            //循环创建新对象
            foreach (var item in xyz)
            {
                var pos = new Vector3(item.x * distanceX, item.y * distanceY, item.z * distanceZ);
#if UNITY_EDITOR
                var newItem = PrefabUtility.InstantiatePrefab(prefabToCopy) as GameObject;
                newItem.transform.SetParent(copiedCollectionObject.transform, false);
#else
                var newItem = Instantiate(prefabToCopy, copiedCollectionObject.transform, false);
#endif
                if (useContainerRotation)
                {
                    newItem.transform.rotation = this.transform.rotation;
                }
                newItem.transform.localPosition = pos;
            }
            Debug.Log("阵列完成");
        }

        public void Extract()
        {
            if (copiedCollectionObject == null)
            {
                Debug.LogWarning("没有创建的对象");
                return;
            }

            var copied = copiedCollectionObject;
            copiedCollectionObject = null;

            copied.hideFlags ^= HideFlags.HideInHierarchy;
            copied.transform.SetParent(null);

            Debug.Log("提取完成");
        }
    }
}