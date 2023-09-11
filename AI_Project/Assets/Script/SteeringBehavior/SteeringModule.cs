using UnityEngine;

public class SteeringModule : MonoBehaviour
{
    public enum SpeedControl
    {
        Slow,
        Normal,
        Fast
    }

    public float slowSpeed = 1.0f;
    public float normalSpeed = 2.0f;
    public float fastSpeed = 3.0f;
    private SpeedControl currentSpeedControl;
    private Vector2 currentVelocity;

    public Vector2 Seek(Vector2 targetPosition)
    {
        float maxSpeed = GetMaxSpeed();
        Vector2 desiredVelocity = (targetPosition - (Vector2)transform.position).normalized * maxSpeed;
        Vector2 steeringForce = desiredVelocity - currentVelocity;
        return steeringForce;
    }
    public void SetSpeedControl(SpeedControl newSpeedControl)
    {
        currentSpeedControl = newSpeedControl;
    }
    private float GetMaxSpeed()
    {
        switch (currentSpeedControl)
        {
            case SpeedControl.Slow:
                return slowSpeed;
            case SpeedControl.Normal:
                return normalSpeed;
            case SpeedControl.Fast:
                return fastSpeed;
            default:
                return normalSpeed;
        }
    }
}
