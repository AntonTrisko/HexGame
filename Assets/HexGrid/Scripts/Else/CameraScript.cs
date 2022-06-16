using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private float _mainSpeed = 100.0f;
    [SerializeField]
    private float _scrollSpeed;
    [SerializeField]
    private float _minY;
    [SerializeField]
    private float _maxY;
    private float _totalRun = 1.0f;

    void Update()
    {
        if (WindowManager.Instance.WindowIsOpen())
        {
            return;
        }
        if (Application.platform.Equals(RuntimePlatform.Android))
        {
            MoveWithTouch();
            ZoomWithTouch();
        }
        else
        {
            MoveWithWASD();
            ZoomWithMouse();
        }
    }

    private void MoveWithTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 delta = touch.deltaPosition * 0.1f;
            transform.position += new Vector3(-delta.x, 0, -delta.y);
        }
    }

    private void ZoomWithTouch()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            Vector2 prevPos0 = touch0.position - touch0.deltaPosition;
            Vector2 prevPos1 = touch1.position - touch1.deltaPosition;
            float oldMagnitude = (prevPos0 - prevPos1).magnitude;
            float newMagnitude = (touch0.position - touch1.position).magnitude;
            float diff = newMagnitude - oldMagnitude;
            Zoom(diff * -0.1f);
        }
    }

    private void ZoomWithMouse()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            Zoom(Input.mouseScrollDelta.y * -_scrollSpeed);
        }
    }

    private void Zoom(float zoom)
    {
        Vector3 newPos = transform.position;
        float posY = newPos.y;
        posY += zoom;
        posY = Mathf.Clamp(posY, _minY, _maxY);
        newPos.y = posY;
        transform.position = newPos;
    }

    private void MoveWithWASD()
    {
        Vector3 p = GetBaseInput();
        if (p.sqrMagnitude > 0)
        {
            _totalRun = Mathf.Clamp(_totalRun * 0.5f, 1f, 1000f);
            p = p * _mainSpeed;
            p = p * Time.deltaTime;
            Vector3 newPosition = transform.position;
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
    }

    private Vector3 GetBaseInput()
    {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}