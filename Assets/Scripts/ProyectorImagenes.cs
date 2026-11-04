using UnityEngine;
using System.Collections;

public class ProyectorImagenes : MonoBehaviour
{
    [Header("Configuración Visual")]
    [Tooltip("Arrastra aquí el objeto de la pantalla blanca")]
    public Renderer pantallaRenderer;
    [Tooltip("Carga aquí tus 4 imágenes")]
    public Texture2D[] imagenes;
    [Tooltip("Tiempo en segundos que dura cada imagen en pantalla")]
    public float tiempoPorImagen = 0.5f;

    [Header("Audio del Proyector (Bloques C y D)")]
    [Tooltip("Arrastra aquí el AudioSource del proyector")]
    public AudioSource audioProyector;
    public AudioClip clipCambioDiapositiva;

    private int indiceActual = 0;
    private bool proyectando = false;


    public void IniciarProyeccion()
    {
        if (!proyectando && imagenes.Length > 0)
        {
            proyectando = true;
            StartCoroutine(CicloImagenes());
        }
    }

    private IEnumerator CicloImagenes()
    {
        while (proyectando)
        {
            // Cambiar la textura (URP usa "_BaseMap" para la textura principal)
            if (pantallaRenderer != null)
            {
                pantallaRenderer.material.SetTexture("_BaseMap", imagenes[indiceActual]);
            }

            // Reproducir el "clic" mecánico con variación de pitch (Bloque C)
            if (audioProyector != null && clipCambioDiapositiva != null)
            {
                audioProyector.pitch = Random.Range(0.85f, 1.15f);
                audioProyector.PlayOneShot(clipCambioDiapositiva);
            }

            // Avanzar al siguiente índice y reiniciar si llega al final
            indiceActual++;
            if (indiceActual >= imagenes.Length)
            {
                indiceActual = 0;
            }

            // Esperar antes de cambiar la siguiente imagen
            yield return new WaitForSeconds(tiempoPorImagen);
        }
    }
}