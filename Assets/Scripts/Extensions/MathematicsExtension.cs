using Unity.Mathematics;
using UnityEngine;

public static class MathematicsExtension
{
    public static float3 ToEulerAngles(this quaternion q) 
    {
        float3 angles;
 
        // roll (x-axis rotation)
        double sinrCosp = 2 * (q.value.w * q.value.x + q.value.y * q.value.z);
        double cosrCosp = 1 - 2 * (q.value.x * q.value.x + q.value.y * q.value.y);
        angles.x = (float)math.atan2(sinrCosp, cosrCosp);
 
        // pitch (y-axis rotation)
        double sinp = 2 * (q.value.w * q.value.y - q.value.z * q.value.x);
        if (math.abs(sinp) >= 1)
            angles.y = (float)CopySign(math.PI / 2, sinp); // use 90 degrees if out of range
        else
            angles.y = (float)math.asin(sinp);
 
        // yaw (z-axis rotation)
        double sinyCosp = 2 * (q.value.w * q.value.z + q.value.x * q.value.y);
        double cosyCosp = 1 - 2 * (q.value.y * q.value.y + q.value.z * q.value.z);
        angles.z = (float)math.atan2(sinyCosp, cosyCosp);
 
        return angles;
    }

    private static double CopySign(double a, double b) 
    {
        return math.abs(a) * math.sign(b);
    }
    
    public static quaternion ClampX(this quaternion q, float min, float max)
    {
        float3 euler = q.ToEulerAngles();
        euler.x = ClampedRotation(euler.x,min,max);
        return Quaternion.Euler(euler);
    }

    public static quaternion ClampY(this quaternion q, float min, float max)
    {
        float3 euler = q.ToEulerAngles();
        euler.y = ClampedRotation(euler.y,min,max);
        return Quaternion.Euler(euler);
    }
    
    public static quaternion ClampZ(this quaternion q, float min, float max)
    {
        float3 euler = q.ToEulerAngles();
        euler.z = ClampedRotation(euler.z,min, max);
        return Quaternion.Euler(euler);
    }

    public static quaternion ClampX(this quaternion q, Vector2 limit)
    {
        return q.ClampX(limit.x, limit.y);
    }

    public static quaternion ClampY(this quaternion q, Vector2 limit)
    {
        return q.ClampY(limit.x, limit.y);
    }

    public static quaternion ClampZ(this quaternion q, Vector2 limit)
    {
        return q.ClampZ(limit.x, limit.y);
    }
    
    private static float ClampedRotation(this float rotation, float min, float max)
    {
        Debug.Log("Old rotation: " + rotation);
        var newRot = math.clamp(rotation.NormalizedRotation(), min.NormalizedRotation(), max.NormalizedRotation());
        Debug.Log("New rotation: " + newRot);
        return newRot;
    }

    private static float NormalizedRotation(this float rotation)
    {
        rotation = rotation % 360;
        return rotation < 0 ? rotation + 360 : rotation;

    }
    
}
