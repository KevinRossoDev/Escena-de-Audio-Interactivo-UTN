using UnityEngine;
using System.Collections;

public class CrossfadeMusicaTrigger : MonoBehaviour
{
    [Header("Pistas de Música")]
    public AudioSource musicaNormal;
    [Tooltip("Arrastrá el objeto invisible con la nueva música de tensión")]
    public AudioSource musicaTension;

    [Header("Configuración del Fade")]
    [Tooltip("Tiempo en segundos que tarda en mezclarse una canción con la otra")]
    public float tiempoDeTransicion = 3.0f;
    [Tooltip("El volumen final que va a tener la música de tensión (de 0 a 1)")]
    public float volumenMaximoTension = 1.0f;

    private bool yaCambio = false;

    private void OnTriggerEnter(Collider other)
    {
        // Solo reacciona al Player y se asegura de ejecutarse una sola vez
        if (other.CompareTag("Player") && !yaCambio)
        {
            yaCambio = true;

            // Inicia la corrutina que hace la matemática del volumen
            StartCoroutine(HacerCrossfade());
        }
    }

    private IEnumerator HacerCrossfade()
    {
        // 1. Despertamos la música de tensión en completo silencio
        if (musicaTension != null)
        {
            musicaTension.volume = 0f;
            musicaTension.Play();
        }

        // 2. Guardamos en qué volumen estaba la música normal por si no estaba al 100%
        float volumenInicialNormal = musicaNormal != null ? musicaNormal.volume : 1f;
        float tiempoPasado = 0f;

        // 3. El ciclo que mezcla gradualmente ambas pistas frame a frame
        while (tiempoPasado < tiempoDeTransicion)
        {
            tiempoPasado += Time.deltaTime;
            float porcentaje = tiempoPasado / tiempoDeTransicion;

            // Bajamos la normal a 0
            if (musicaNormal != null)
                musicaNormal.volume = Mathf.Lerp(volumenInicialNormal, 0f, porcentaje);

            // Subimos la tensión a su valor máximo
            if (musicaTension != null)
                musicaTension.volume = Mathf.Lerp(0f, volumenMaximoTension, porcentaje);

            yield return null;
        }

        // 4. Forzamos los valores exactos al terminar y apagamos la pista vieja
        if (musicaNormal != null)
        {
            musicaNormal.volume = 0f;
            musicaNormal.Stop(); // La apagamos para no gastar memoria procesando silencio
        }
        if (musicaTension != null)
        {
            musicaTension.volume = volumenMaximoTension;
        }
    }
}