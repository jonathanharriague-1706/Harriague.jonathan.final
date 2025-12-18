using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Salud")]
    public int maxVida = 28;
    private int vidaActual;
    public Slider sliderVida;

    [Header("Disparo")]
    public GameObject balaPrefab;
    public Transform puntoDisparo;

    [Header("Movimiento")]
    public float velocidad = 8f;
    public float fuerzaSalto = 12f;
    private Rigidbody rb;

    [Header("Suelo")]
    public Transform groundCheck;
    public LayerMask capaSuelo;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        vidaActual = maxVida;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        if (sliderVida != null)
        {
            sliderVida.maxValue = maxVida;
            sliderVida.value = maxVida;
        }
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector3(inputX * velocidad, rb.linearVelocity.y, 0);

        if (inputX > 0) transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (inputX < 0) transform.rotation = Quaternion.Euler(0, -90, 0);

        if (Physics.CheckSphere(groundCheck.position, 0.3f, capaSuelo) && Input.GetButtonDown("Jump"))
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);

        if (Input.GetKeyDown(KeyCode.Z))
            Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);
    }

    // He puesto "UnityEngine.Debug" para que nunca m�s te salga ese error
    public void RecibirDano(int cantidad)
    {
        vidaActual -= cantidad;
        if (sliderVida != null) sliderVida.value = vidaActual;

        UnityEngine.Debug.Log("Vida Jugador: " + vidaActual);

        if (vidaActual <= 0)
        {
            UnityEngine.Debug.Log("Jugador Muerto");
            gameObject.SetActive(false);
        }
    }
}