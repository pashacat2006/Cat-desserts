using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce = 25f;     // сила движения
    public float maxVelocity = 8f;    // лимит скорости
    public float jumpImpulse = 6f;    // сила прыжка (импульс)

    [Header("Mouse Look")]
    public float sensitivityX = 15f;
    public float sensitivityY = 15f;
    public float minY = -60f;
    public float maxY = 60f;

    private Rigidbody rb;
    private float rotY;
    private bool canJump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    // Если нужно "пока касается", можно оставить и это:
    private void OnCollisionStay(Collision collision)
    {
        canJump = true;
    }

    // Если вместо коллизий ты используешь Trigger'ы — включи isTrigger и используй это:
    private void OnTriggerEnter(Collider other)
    {
        canJump = true;
        // Debug.Log("Trigger: " + other.name);
    }

    private void OnTriggerStay(Collider other)
    {
        canJump = true;
    }

    void Update()
    {
        // Поворот мышью
        float rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        rotY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotY = Mathf.Clamp(rotY, minY, maxY);

        transform.localEulerAngles = new Vector3(-rotY, rotX, 0);
    }

    void FixedUpdate()
    {
        // Движение WASD
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) dir += transform.forward;
        if (Input.GetKey(KeyCode.S)) dir -= transform.forward;
        if (Input.GetKey(KeyCode.D)) dir += transform.right;
        if (Input.GetKey(KeyCode.A)) dir -= transform.right;

        if (dir.sqrMagnitude > 0f)
            rb.AddForce(dir.normalized * moveForce, ForceMode.Force);

        // Прыжок (разрешён, если касались чего-то)
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
            canJump = false;
        }

        // Ограничение скорости
        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = rb.velocity.normalized * maxVelocity;
    }
}