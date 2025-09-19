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

    [SerializeField]
    [Tooltip("Scales the additional velocity imparted to the ball when the flipper is moving towards it.")]
    private float collisionBoostMultiplier = 1.35f;

    [SerializeField]
    [Tooltip("Minimum angular speed (in degrees per second) before collision boosting is applied.")]
    private float minimumBoostSpeed = 120f;

    private Quaternion _baseRotation;
    private Vector3 _normalizedAxis;
    private float _currentAngle;
    private float _targetAngle;
    private Rigidbody _rigidbody;
    private float _previousAngle;
    private float _angularVelocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _baseRotation = transform.localRotation;
        _normalizedAxis = rotationAxis.sqrMagnitude > Mathf.Epsilon
            ? rotationAxis.normalized
            : Vector3.forward;
        _currentAngle = restAngle;
        _targetAngle = restAngle;
        _previousAngle = restAngle;
        _angularVelocity = 0f;
    }

    private void Update()
    {
        _targetAngle = Input.GetKey(activationKey) ? activeAngle : restAngle;
    }

    private void FixedUpdate()
    {
        if (Mathf.Approximately(_currentAngle, _targetAngle))
        {
            _previousAngle = _currentAngle;
            _angularVelocity = 0f;
            return;
        }

        _previousAngle = _currentAngle;
        _currentAngle = Mathf.MoveTowards(
            _currentAngle,
            _targetAngle,
            moveSpeed * Time.fixedDeltaTime);
        _angularVelocity = (_currentAngle - _previousAngle) * Mathf.Deg2Rad / Time.fixedDeltaTime;
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
    }

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody otherBody = collision.rigidbody;
        if (otherBody == null)
        {
            return;
        }

        if (Mathf.Abs(_angularVelocity) < minimumBoostSpeed * Mathf.Deg2Rad)
        {
            return;
        }

        Vector3 worldAxis = transform.TransformDirection(_normalizedAxis);

        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 contactVector = contact.point - transform.position;
            if (contactVector.sqrMagnitude < Mathf.Epsilon)
            {
                continue;
            }

            Vector3 flipperVelocity = Vector3.Cross(worldAxis * _angularVelocity, contactVector);
            if (flipperVelocity.sqrMagnitude < Mathf.Epsilon)
            {
                continue;
            }

            Vector3 direction = flipperVelocity.normalized;
            Vector3 ballVelocity = otherBody.GetPointVelocity(contact.point);
            float relativeSpeed = Vector3.Dot(flipperVelocity - ballVelocity, direction);

            if (relativeSpeed <= 0f)
            {
                continue;
            }

            float boost = relativeSpeed * collisionBoostMultiplier;
            otherBody.AddForce(direction * boost * otherBody.mass, ForceMode.Impulse);
        }
    }
}
