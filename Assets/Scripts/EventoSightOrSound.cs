using UnityEngine;
using System.Collections;

public class EventoSightOrSound : MonoBehaviour
{
    [Header("El susto en el escenario")]
    public AudioSource fuenteDeSonido;

    [Header("Reacción del Jugador (Latidos)")]
    public AudioSource latidosPlayer;
    public float pitchTaquicardia = 1.4f;
    public float volumenTension = 1.0f;

   
    public float duracionTaquicardia = 3.0f;
    
    public float tiempoParaCalmarse = 4.0f;

    [Header("Configuración del Evento")]
    public bool sonarSoloUnaVez = true;

    private float pitchNormal;
    private float volumenNormal;

    private void Start()
    {
        if (latidosPlayer != null)
        {
            pitchNormal = latidosPlayer.pitch;
            volumenNormal = latidosPlayer.volume;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fuenteDeSonido != null) fuenteDeSonido.Play();

            if (latidosPlayer != null)
            {
                latidosPlayer.pitch = pitchTaquicardia;
                latidosPlayer.volume = volumenTension;

                StartCoroutine(CalmarLatidosGradual());
            }

            if (sonarSoloUnaVez)
            {
                GetComponent<Collider>().enabled = false;
                Destroy(gameObject, duracionTaquicardia + tiempoParaCalmarse + 1f);
            }
        }
    }

    private IEnumerator CalmarLatidosGradual()
    {
        yield return new WaitForSeconds(duracionTaquicardia);

        if (latidosPlayer != null)
        {
            float tiempoPasado = 0f;
            float pitchInicioFade = latidosPlayer.pitch;
            float volumenInicioFade = latidosPlayer.volume;

            while (tiempoPasado < tiempoParaCalmarse)
            {
                tiempoPasado += Time.deltaTime;
                float porcentaje = tiempoPasado / tiempoParaCalmarse;

                // Mathf.Lerp calcula el valor intermedio suavemente
                latidosPlayer.pitch = Mathf.Lerp(pitchInicioFade, pitchNormal, porcentaje);
                latidosPlayer.volume = Mathf.Lerp(volumenInicioFade, volumenNormal, porcentaje);

                yield return null;
            }

            latidosPlayer.pitch = pitchNormal;
            latidosPlayer.volume = volumenNormal;
        }
    }
}