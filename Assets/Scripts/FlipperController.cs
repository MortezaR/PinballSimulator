using UnityEngine;

public class FlipperController : MonoBehaviour
{
    [SerializeField]
    private KeyCode activationKey = KeyCode.A;

    [SerializeField]
    private float restAngle = 0f;

    [SerializeField]
    private float activeAngle = 55f;

    [SerializeField]
    private float moveSpeed = 600f;

    [SerializeField]
    private Vector3 rotationAxis = Vector3.forward;

    private Quaternion _baseRotation;
    private Vector3 _normalizedAxis;
    private float _currentAngle;

    private void Awake()
    {
        _baseRotation = transform.localRotation;
        _normalizedAxis = rotationAxis.sqrMagnitude > Mathf.Epsilon
            ? rotationAxis.normalized
            : Vector3.forward;
    }

    private void Start()
    {
        _currentAngle = restAngle;
        ApplyRotation(_currentAngle);
    }

    private void Update()
    {
        float targetAngle = Input.GetKey(activationKey) ? activeAngle : restAngle;
        if (!Mathf.Approximately(_currentAngle, targetAngle))
        {
            _currentAngle = Mathf.MoveTowards(
                _currentAngle,
                targetAngle,
                moveSpeed * Time.deltaTime);
            ApplyRotation(_currentAngle);
        }
    }

    private void ApplyRotation(float angle)
    {
        transform.localRotation = Quaternion.AngleAxis(angle, _normalizedAxis) * _baseRotation;
    }
}
