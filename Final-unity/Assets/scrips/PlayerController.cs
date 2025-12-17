using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento Side-Scroller")]
    public float moveSpeed = 8f;
    public float jumpForce = 7f; // Fuerza del salto
    public float fallMultiplier = 2.5f; // Para que el salto se sienta menos "flotante"
    private Rigidbody rb;

    [Header("Detección de Suelo")]
    public Transform groundCheck;
    public float checkRadius = 0.3f;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Ataque y Salud")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public int maxHealth = 3;
    private int currentHealth;
    public Slider healthSlider;
    private Vector3 initialPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        currentHealth = maxHealth;

        // BLOQUEO CRUCIAL PARA 2D EN 3D:
        // Bloqueamos rotaciones y el movimiento en Z (profundidad)
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    void Start()
    {
        if (healthSlider != null) healthSlider.value = 1f;
    }

    void Update()
    {
        Mover();
        Saltar();

        if (Input.GetButtonDown("Fire1")) Atacar();
        if (Input.GetKeyDown(KeyCode.F1)) Respawn();
    }

    void Mover()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        // Solo afectamos X y mantenemos la velocidad actual de Y (caída/salto)
        rb.linearVelocity = new Vector3(inputX * moveSpeed, rb.linearVelocity.y, 0);

        // Rotación estilo Mega Man (90 grados para mirar a la derecha, -90 para la izquierda)
        if (inputX > 0) transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (inputX < 0) transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    void Saltar()
    {
        // Detectamos si estamos tocando el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, whatIsGround);

        // Si presionamos espacio (Jump) y estamos en el suelo
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Mejora de gravedad (opcional para que se sienta mejor el salto)
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void Atacar()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null) bulletRb.linearVelocity = transform.forward * bulletSpeed;
        }
    }

    // --- Métodos de Salud y Respawn ---
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (healthSlider != null) healthSlider.value = (float)currentHealth / maxHealth;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        gameObject.SetActive(false);
        UnityEngine.Debug.Log("Jugador eliminado. Presiona F1.");
    }

    void Respawn()
    {
        transform.position = initialPosition;
        currentHealth = maxHealth;
        if (healthSlider != null) healthSlider.value = 1f;
        gameObject.SetActive(true);
        rb.linearVelocity = Vector3.zero;
    }

    // Para ver el radio del GroundCheck en el editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}