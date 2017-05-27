using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NGUIToolTest
{
    static Vector3[] m_Sides = new Vector3[4];
    static public Vector3[] GetSidesTest(this Camera cam, float depth, Transform relativeTo)
    {
        if(cam.orthographic)
        {
            float os = cam.orthographicSize;
            float x0 = -os;
            float x1 = os;
            float y0 = -os;
            float y1 = os;

            Rect rect = cam.rect;
            Vector2 size = NGUITools.screenSize;

            float aspect = size.x / size.y;
            aspect *= rect.width / rect.height;
            x0 *= aspect;
            x1 *= aspect;

            Transform t = cam.transform;
            Quaternion rot = t.rotation;
            Vector3 pos = t.position;

            int w = Mathf.RoundToInt(size.x);
            int h = Mathf.RoundToInt(size.y);

            if (( w & 1 ) == 1)
                pos.x -= 1f / size.x;
            if (( h & 1 ) == 1)
                pos.y += 1f / size.y;

            m_Sides[0] = rot * (new Vector3 (x0, 0f, depth)) +pos;
            m_Sides[1] = rot * ( new Vector3(0, y1, depth) ) + pos;
            m_Sides[2] = rot * ( new Vector3(x1, 0, depth) ) + pos;
            m_Sides[3] = rot * ( new Vector3(0, y0, depth) ) + pos;


        }
        for(int i = 0; i < m_Sides.Length; i++)
        {
            Debug.LogError(m_Sides[i]);
        }

        if (relativeTo != null)
        {
            for (int i = 0; i < 4; ++i)
            {
                m_Sides[i] = relativeTo.InverseTransformPoint(m_Sides[i]);
                Debug.LogError(m_Sides[i]);
            }
        }
        Debug.LogError(" -------------");
        m_Sides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, depth));
        m_Sides[1] = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, depth));
        m_Sides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, depth));
        m_Sides[3] = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, depth));

        for (int i = 0; i < m_Sides.Length; i++)
        {
            Debug.LogError(m_Sides[i]);
        }

        return m_Sides;
    }

}
