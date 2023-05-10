using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BezierUtility
{
    // 计算贝塞尔曲线长度
    public static float GetBezierLength(ControlPoint cp1, ControlPoint cp2)
    {
        var length = 0f;
        var step = 0.1f;
        var t = 0f;
        var p1 = cp1.pMain;
        var p2 = cp2.pMain;
        while (t < 1)
        {
            var p = GetBezierPoint(p1, cp1.pAfter, cp2.pBefore, p2, t);
            length += Vector3.Distance(p, p1);
            p1 = p;
            t += step;
        }
        return length;
    }
    // 计算贝塞尔曲线上的点
    public static Vector3 GetBezierPoint(ControlPoint cp1, ControlPoint cp2, float t)
    {
        var p1 = cp1.pMain;
        var p2 = cp2.pMain;
        return GetBezierPoint(p1, cp1.pAfter, cp2.pBefore, p2, t);
    }
    // 计算贝塞尔曲线上的点
    public static Vector3 GetBezierPoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        var t1 = Mathf.Pow(1 - t, 3);
        var t2 = 3 * t * Mathf.Pow(1 - t, 2);
        var t3 = 3 * Mathf.Pow(t, 2) * (1 - t);
        var t4 = Mathf.Pow(t, 3);
        return t1 * p1 + t2 * p2 + t3 * p3 + t4 * p4;
    }
    // 计算贝塞尔曲线上的点的切线
    public static Vector3 GetBezierTangent(ControlPoint cp1, ControlPoint cp2, float t)
    {
        var p1 = cp1.pMain;
        var p2 = cp2.pMain;
        return GetBezierTangent(p1, cp1.pAfter, cp2.pBefore, p2, t);
    }
    // 计算贝塞尔曲线上的点的切线
    public static Vector3 GetBezierTangent(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        var t1 = -3 * Mathf.Pow(1 - t, 2);
        var t2 = 3 * (1 - 4 * t + 3 * Mathf.Pow(t, 2));
        var t3 = 3 * (2 * t - 3 * Mathf.Pow(t, 2));
        var t4 = 3 * Mathf.Pow(t, 2);
        return t1 * p1 + t2 * p2 + t3 * p3 + t4 * p4;
    }
    // 求得两侧偏移长度n后的点
    public static Vector3 GetBezierOffsetPoint(ControlPoint cp1, ControlPoint cp2, float t, float n)
    {
        var p1 = cp1.pMain;
        var p2 = cp2.pMain;
        return GetBezierOffsetPoint(p1, cp1.pAfter, cp2.pBefore, p2, t, n);
    }
    // 求得两侧偏移长度n后的点
    public static Vector3 GetBezierOffsetPoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t, float n)
    {
        var tangent = GetBezierTangent(p1, p2, p3, p4, t);
        //TODO: 此处Normal计算有问题，不一定使用向上的叉乘
        var normal = Vector3.Cross(tangent, Vector3.up).normalized;
        return GetBezierPoint(p1, p2, p3, p4, t) + normal * n;
    }
}
