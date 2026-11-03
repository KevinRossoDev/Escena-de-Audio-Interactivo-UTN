using UnityEngine;
using System.Collections;

public class EventoSightOrSound : MonoBehaviour
{
    [Header("El susto en el escenario")]
    public AudioSource fuenteDeSonido;

    [Header("Reacción del Jugador (Latidos)")]
    public AudioSource latidosPlayer;
    public float pitchTaquicardia = 1.5f;
    public float volumenTension = 1.0f;

    [Header("Reacción del Jugador (Respiración)")]
    public AudioSource respiracionPlayer;
    public AudioClip respiracionAgitada;
    private AudioClip respiracionNormal; // El script lo anota solo al arrancar

    [Header("Tiempos")]
    public float duracionPico = 2.0f;
    public float tiempoDeRecuperacion = 4.0f;

    [Header("Configuración del Evento")]
    public bool sonarSoloUnaVez = true;

    private float pitchNormal;
    private float volumenNormal;

    private void Start()
    {
        // Guardamos los valores y audios normales para poder volver a ellos después
        if (latidosPlayer != null)
        {
            pitchNormal = latidosPlayer.pitch;
            volumenNormal = latidosPlayer.volume;
        }

        if (respiracionPlayer != null)
        {
            respiracionNormal = respiracionPlayer.clip;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fuenteDeSonido != null) fuenteDeSonido.Play();

            // 1. Cambiamos la respiración al clip agitado
            if (respiracionPlayer != null && respiracionAgitada != null)
            {
                respiracionPlayer.clip = respiracionAgitada;
                respiracionPlayer.Play();
            }

            // 2. Aceleramos el corazón
            if (latidosPlayer != null)
            {
                latidosPlayer.pitch = pitchTaquicardia;
                latidosPlayer.volume = volumenTension;
                StartCoroutine(CalmarLatidosGradual());
            }

            if (sonarSoloUnaVez)
            {
                GetComponent<Collider>().enabled = false;
                Destroy(gameObject, duracionPico + tiempoDeRecuperacion + 1f);
            }
        }
    }

    private IEnumerator CalmarLatidosGradual()
    {
        yield return new WaitForSeconds(duracionPico);

        float tiempoPasado = 0f;
        while (tiempoPasado < tiempoDeRecuperacion)
        {
            tiempoPasado += Time.deltaTime;
            float porcentaje = tiempoPasado / tiempoDeRecuperacion;

            latidosPlayer.pitch = Mathf.Lerp(pitchTaquicardia, pitchNormal, porcentaje);
            latidosPlayer.volume = Mathf.Lerp(volumenTension, volumenNormal, porcentaje);

            yield return null;
        }

       
        latidosPlayer.pitch = pitchNormal;
        latidosPlayer.volume = volumenNormal;

        
        if (respiracionPlayer != null && respiracionNormal != null)
        {
            respiracionPlayer.clip = respiracionNormal;
            respiracionPlayer.Play();
        }
    }
}