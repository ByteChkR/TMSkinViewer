using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefabSpawnHelper
{

    private static float m_Offset = 1000;
    private static float m_Current = 0;

    public static Vector3 GetSpawn()
    {
        Vector3 p = new Vector3( 0, 0, m_Current );
        m_Current += m_Offset;

        return p;
    }

}
