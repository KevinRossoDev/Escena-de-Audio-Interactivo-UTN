using UnityEngine;

public class EncenderProyectorTrigger : MonoBehaviour
{
    [Header("El proyector")]
    public ProyectorImagenes scriptDelProyector;

    [Header("El Sonido de Encendido")]
    public AudioSource audioEncendidoInicial;

    private bool yaSeActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaSeActivo)
        {
            yaSeActivo = true;

            if (audioEncendidoInicial != null)
            {
                audioEncendidoInicial.Play();
            }

            if (scriptDelProyector != null)
            {
                scriptDelProyector.IniciarProyeccion();
            }

            gameObject.SetActive(false);
        }
    }
}