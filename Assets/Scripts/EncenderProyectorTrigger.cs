using UnityEngine;

public class EncenderProyectorTrigger : MonoBehaviour
{
    [Header("El Script de las Imágenes")]
    [Tooltip("Arrastra aquí la pantalla que tiene el script ProyectorImagenes")]
    public ProyectorImagenes scriptDelProyector;

    [Header("El Sonido de Encendido Inicial")]
    [Tooltip("Arrastra aquí el AudioSource que tiene el ruido del motor arrancando")]
    public AudioSource audioEncendidoInicial;

    private bool yaSeActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaSeActivo)
        {
            yaSeActivo = true;

            // 1. Reproduce el sonido de "arranque" del proyector
            if (audioEncendidoInicial != null)
            {
                audioEncendidoInicial.Play();
            }

            // 2. "Despierta" el script que pasa las fotos y hace los clics
            if (scriptDelProyector != null)
            {
                scriptDelProyector.IniciarProyeccion();
            }

            // 3. Apagamos este cubo invisible para no volver a pisarlo
            gameObject.SetActive(false);
        }
    }
}