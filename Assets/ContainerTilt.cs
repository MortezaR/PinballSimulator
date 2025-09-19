using UnityEngine;

[ExecuteAlways]
public class ContainerTilt : MonoBehaviour
{
    [Tooltip("Tilt angle of the container in degrees.")]
    public float tilt = 20f;

    private void OnEnable()
    {
        ApplyTilt();
    }

    private void OnValidate()
    {
        ApplyTilt();
    }

    private void ApplyTilt()
    {
        transform.localRotation = Quaternion.Euler(tilt, 0f, 0f);
    }
}
