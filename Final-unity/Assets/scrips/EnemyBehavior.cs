using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public int vidaEnemigo = 3;
    public int danoAlJugador = 4;
    public float velocidad = 3f;
    public float rangoPatrulla = 4f;

    private Vector3 inicio;
    private int dir = 1;

    void Start() { inicio = transform.position; }

    void Update()
    {
        transform.Translate(Vector3.right * dir * velocidad * Time.deltaTime);
        if (Vector3.Distance(inicio, transform.position) > rangoPatrulla) dir *= -1;
    }

    public void TomarDano(int d)
    {
        vidaEnemigo -= d;
        if (vidaEnemigo <= 0) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detectamos al jugador por su Tag
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController scriptJugador = collision.gameObject.GetComponent<PlayerController>();

            if (scriptJugador != null)
            {
                // Llamamos a la función exacta del script de arriba
                scriptJugador.RecibirDano(danoAlJugador);
            }
        }
    }
}