using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonComponent : MonoBehaviour
{
    public static PersonComponent Instance;
    const float k_MouseSensitivityMultiplier = 0.01f;
    public float mouseSensitivity = 60.0f;
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));
    public float rotationLerpTime = 0.01f;
    public bool invertY = false;
    public float boost = 3.5f;
    public float positionLerpTime = 0.2f;
    class ComponentState
    {
        public float yaw;
        public float pitch;
        public float roll;
        public float x;
        public float y;
        public float z;

        public void SetFromTransform(Transform t)
        {
            pitch = t.eulerAngles.x;
            yaw = t.eulerAngles.y;
            roll = t.eulerAngles.z;
            x = t.position.x;
            y = t.position.y;
            z = t.position.z;
        }

        public void Translate(Vector3 translation)
        {
            Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

            x += rotatedTranslation.x;
            y += rotatedTranslation.y;
            z += rotatedTranslation.z;
        }

        public void LerpTowards(ComponentState target, float positionLerpPct, float rotationLerpPct)
        {
            yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
            pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
            roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

            x = Mathf.Lerp(x, target.x, positionLerpPct);
            y = Mathf.Lerp(y, target.y, positionLerpPct);
            z = Mathf.Lerp(z, target.z, positionLerpPct);
        }

        public void UpdateTransform(Transform t)
        {
            t.eulerAngles = new Vector3(pitch, yaw, roll);
            t.position = new Vector3(x, y, z);
        }
    }
    ComponentState _TargetState = new ComponentState();
    ComponentState _InterpolatingState = new ComponentState();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEscapePressed())
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        if (IsRightMouseButtonDown())
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (IsRightMouseButtonUp())
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (IsCameraRotationAllowed())
        {
            var mouseMovement = GetInputLookRotation() * k_MouseSensitivityMultiplier * mouseSensitivity;
            if (invertY)
                mouseMovement.y = -mouseMovement.y;

            var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

            _TargetState.yaw += mouseMovement.x * mouseSensitivityFactor;
            _TargetState.pitch += mouseMovement.y * mouseSensitivityFactor;
        }
        var translation = GetInputTranslationDirection() * Time.deltaTime;
        if (IsBoostPressed())
        {
            translation *= 10.0f;
        }
        boost += GetBoostFactor();
        translation *= Mathf.Pow(2.0f, boost);
        _TargetState.Translate(translation);
        var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
        var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
        _InterpolatingState.LerpTowards(_TargetState, positionLerpPct, rotationLerpPct);
        _InterpolatingState.UpdateTransform(transform);
    }
    bool IsEscapePressed()
    {
        return Input.GetKey(KeyCode.Escape);
    }
    bool IsRightMouseButtonDown()
    {
        return Input.GetMouseButtonDown(1);
    }
    bool IsRightMouseButtonUp()
    {
        return Input.GetMouseButtonUp(1);
    }
    bool IsCameraRotationAllowed()
    {
        return Input.GetMouseButton(1);
    }
    Vector2 GetInputLookRotation()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
    bool IsBoostPressed()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
    Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction += Vector3.down;
        }
        if (Input.GetKey(KeyCode.E))
        {
            direction += Vector3.up;
        }
        return direction;
    }
    float GetBoostFactor()
    {
        return Input.mouseScrollDelta.y * 0.01f;
    }
}
