using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float moveSpeed = 8f;
    public float jumpForce = 10f;
    private Rigidbody rb;

    [Header("Detección de Suelo")]
    public Transform groundCheck; // Arrastra aquí el objeto 'Pies'
    public float checkRadius = 0.3f;
    public LayerMask whatIsGround; // Selecciona la capa de tu suelo
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // BLOQUEO DE EJES: Evita movimiento en Z (fondo) y rotaciones indeseadas
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        // Mejora la fluidez visual del movimiento
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        Mover();
        Saltar();
    }

    void Mover()
    {
        // Forzamos el uso del eje X (Horizontal)
        float inputX = Input.GetAxisRaw("Horizontal");

        // Aplicamos velocidad solo en X, mantenemos Y (caída/salto) y 0 en Z
        rb.linearVelocity = new Vector3(inputX * moveSpeed, rb.linearVelocity.y, 0);

        // Girar el personaje para que mire a la izquierda o derecha
        if (inputX > 0) transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (inputX < 0) transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    void Saltar()
    {
        if (groundCheck == null) return;

        // Detecta si el círculo en los pies toca la capa de suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, whatIsGround);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // Reset de velocidad vertical antes del impulso para saltos consistentes
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Dibuja una esfera en el editor para que veas dónde detecta el suelo
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}