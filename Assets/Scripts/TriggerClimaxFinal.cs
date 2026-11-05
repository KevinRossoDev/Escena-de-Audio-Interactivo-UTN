using UnityEngine;
using System.Collections;

public class TriggerClimaxFinal : MonoBehaviour
{
    [Header("1. Conexión con Proyector")]
    public ProyectorImagenes proyector;
    public Texture2D[] fotosCrucifixion;

    [Header("2. Transición de Respiración")]
    public AudioSource respiracionNormal;
    public AudioSource respiracionAgitada;

    [Header("3. Efecto Visual de Final")]
    public Camera camaraJugador;
    public CanvasGroup pantallaBlanca;
    public float intensidadTemblor = 0.2f;

    [Header("4. Tiempos Independientes")]
    [Tooltip("Segundos que tardan el audio y el temblor en llegar a su máximo nivel de estrés")]
    public float duracionAudioYTemblor = 3.0f;
    [Tooltip("Segundos que tarda la pantalla en ponerse totalmente blanca (Hacelo más largo)")]
    public float duracionFadeBlanco = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (proyector != null) proyector.CambiarListaDeImagenes(fotosCrucifixion);
            StartCoroutine(SecuenciaFinalClimax());
            GetComponent<Collider>().enabled = false;
        }
    }

    private IEnumerator SecuenciaFinalClimax()
    {
        if (respiracionAgitada != null)
        {
            respiracionAgitada.volume = 0f;
            respiracionAgitada.Play();
        }

        float volInicialNorm = respiracionNormal != null ? respiracionNormal.volume : 1f;
        Vector3 posOriginalCamara = camaraJugador != null ? camaraJugador.transform.localPosition : Vector3.zero;

        float tiempoPasado = 0f;
        // Calculamos cuál de los dos tiempos es el más largo para saber cuándo terminar el bucle
        float tiempoMaximo = Mathf.Max(duracionAudioYTemblor, duracionFadeBlanco);

        while (tiempoPasado < tiempoMaximo)
        {
            tiempoPasado += Time.deltaTime;

            // Calculamos dos porcentajes separados (Clamp01 asegura que nunca pasen del 100%)
            float pctClimax = Mathf.Clamp01(tiempoPasado / duracionAudioYTemblor);
            float pctBlanco = Mathf.Clamp01(tiempoPasado / duracionFadeBlanco);

            // A. Audio y Temblor (usan el tiempo rápido)
            if (respiracionNormal != null) respiracionNormal.volume = Mathf.Lerp(volInicialNorm, 0f, pctClimax);
            if (respiracionAgitada != null) respiracionAgitada.volume = Mathf.Lerp(0f, 1f, pctClimax);
            if (camaraJugador != null)
            {
                float fuerzaActual = Mathf.Lerp(0f, intensidadTemblor, pctClimax);
                camaraJugador.transform.localPosition = posOriginalCamara + Random.insideUnitSphere * fuerzaActual;
            }

            // B. Halo Incandescente (usa el tiempo lento)
            if (pantallaBlanca != null) pantallaBlanca.alpha = pctBlanco;

            yield return null;
        }

        // Forzamos valores finales al terminar
        if (camaraJugador != null) camaraJugador.transform.localPosition = posOriginalCamara;
        if (pantallaBlanca != null) pantallaBlanca.alpha = 1f;
        if (respiracionNormal != null) respiracionNormal.volume = 0f;
        if (respiracionAgitada != null) respiracionAgitada.volume = 1f;
    }
}