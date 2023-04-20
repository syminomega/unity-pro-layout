using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProLayout
{
    public class ArrayCopy : MonoBehaviour
    {
        // 需要拷贝的对象
        public GameObject prefabToCopy;

        // 存放拷贝对象的集合
        [SerializeField] [HideInInspector] private GameObject copiedCollectionObject;

        public float distanceX;
        [Min(1)] public int countX = 1;

        public float distanceY;
        [Min(1)] public int countY = 1;

        public float distanceZ;
        [Min(1)] public int countZ = 1;

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
                var newItem = Instantiate(prefabToCopy, copiedCollectionObject.transform, true);
                newItem.transform.localPosition = pos;
            }
        }
    }
}