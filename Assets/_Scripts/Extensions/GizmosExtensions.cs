using UnityEngine;

public static class GizmosExtensions 
{
    public static void DrawCapsule(Vector3 center, float radius, float height)
    {
        Vector3 topCenter = center + Vector3.up * (height / 2 - radius);
        Gizmos.DrawWireSphere(topCenter, radius);

        Vector3 bottomCenter = center - Vector3.up * (height / 2 - radius);
        Gizmos.DrawWireSphere(bottomCenter, radius);

        Vector3 p1, p2;
        for (float angle = 0; angle <= 360; angle += 10)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            p1 = topCenter + new Vector3(x, 0, z);
            p2 = bottomCenter + new Vector3(x, 0, z);
            Gizmos.DrawLine(p1, p2);
        }
    }
}