using UnityEngine;
using System.Collections;

public class ProyectorImagenes : MonoBehaviour
{
    [Header("Configuración Visual")]
    [Tooltip("Arrastra aquí el objeto de la pantalla blanca")]
    public Renderer pantallaRenderer;
    [Tooltip("Imagenes")]
    public Texture2D[] imagenes;
    [Tooltip("Tiempor entre imagenes")]
    public float tiempoPorImagen = 0.5f;

  
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
            if (pantallaRenderer != null)
            {
                pantallaRenderer.material.SetTexture("_BaseMap", imagenes[indiceActual]);
            }

            if (audioProyector != null && clipCambioDiapositiva != null)
            {
                audioProyector.pitch = Random.Range(0.85f, 1.15f);
                audioProyector.PlayOneShot(clipCambioDiapositiva);
            }

            indiceActual++;
            if (indiceActual >= imagenes.Length)
            {
                indiceActual = 0;
            }

            yield return new WaitForSeconds(tiempoPorImagen);
        }
    }
    public void CambiarListaDeImagenes(Texture2D[] nuevasImagenes)
    {
        if (nuevasImagenes.Length > 0)
        {
            imagenes = nuevasImagenes; 
            indiceActual = 0;          
        }
    }
}