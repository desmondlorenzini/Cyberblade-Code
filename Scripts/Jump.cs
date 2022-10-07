using UnityEngine;

[System.Serializable]
public class Jump : MonoBehaviour
{
    public float jumpForce;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.5f, groundLayer);

        if ( Inputs.bButtonPressed && isGrounded && rb.velocity.y < 5)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
