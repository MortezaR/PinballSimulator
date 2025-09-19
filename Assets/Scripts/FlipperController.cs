using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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
    private float _targetAngle;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _baseRotation = transform.localRotation;
        _normalizedAxis = rotationAxis.sqrMagnitude > Mathf.Epsilon
            ? rotationAxis.normalized
            : Vector3.forward;
        _currentAngle = restAngle;
        _targetAngle = restAngle;
    }

    private void Update()
    {
        _targetAngle = Input.GetKey(activationKey) ? activeAngle : restAngle;
    }

    private void FixedUpdate()
    {
        if (Mathf.Approximately(_currentAngle, _targetAngle))
        {
            return;
        }

        _currentAngle = Mathf.MoveTowards(
            _currentAngle,
            _targetAngle,
            moveSpeed * Time.fixedDeltaTime);
        ApplyRotation(_currentAngle, useMoveRotation: true);
    }

    private void OnEnable()
    {
        ApplyRotation(_currentAngle, useMoveRotation: false);
    }

    private void ApplyRotation(float angle, bool useMoveRotation)
    {
        Quaternion localRotation = Quaternion.AngleAxis(angle, _normalizedAxis) * _baseRotation;
        Quaternion targetRotation = transform.parent != null
            ? transform.parent.rotation * localRotation
            : localRotation;

        if (useMoveRotation)
        {
            _rigidbody.MoveRotation(targetRotation);
        }
        else
        {
            _rigidbody.rotation = targetRotation;
        }
        print(localRotation);
    }
}
