using UnityEngine;

public class BalaBehavior : MonoBehaviour
{
    public float velocidadInicial = 20f;
    public float aceleracion = 10f; // Cuánto aumenta la velocidad por segundo
    public float tiempoVida = 2f;

    private float velocidadActual;

    void Start()
    {
        velocidadActual = velocidadInicial;
        // Forzamos la destrucción desde el inicio
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        // 1. Aplicamos aceleración
        velocidadActual += aceleracion * Time.deltaTime;

        // 2. Movimiento: Usamos transform.forward para asegurar la dirección global
        transform.position += transform.forward * velocidadActual * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Evitamos que la bala se choque con el propio jugador al nacer
        if (other.CompareTag("Player")) return;

        // Si toca cualquier otra cosa, desaparece
        Destroy(gameObject);
    }
}