using UnityEngine;
using System.Collections;

public class LucesFlickerTension : MonoBehaviour
{
    [Header("Luces a controlar")]
    public Light[] luces;

    [Header("Colores del Parpadeo")]
    [Tooltip("El color normal de la luz")]
    public Color colorNormal = Color.white;
    [Tooltip("El color de la falla (ej. Rojo sangre)")]
    public Color colorAlerta = Color.red;

    [Header("Audio del Parpadeo (Bloques C y D)")]
    public AudioClip clipChispa;

    [Header("Tiempos de Parpadeo")]
    public float minTiempoPrendida = 0.05f;
    public float maxTiempoPrendida = 0.3f;
    public float minTiempoApagada = 0.05f;
    public float maxTiempoApagada = 0.4f;

    private bool efectoActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !efectoActivo)
        {
            efectoActivo = true;

            foreach (Light luz in luces)
            {
                if (luz != null) StartCoroutine(EfectoFlickerIndividual(luz));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            efectoActivo = false;
            foreach (Light luz in luces)
            {
                if (luz != null)
                {
                    // Cuando el jugador se va, dejamos todo como estaba
                    luz.enabled = true;
                    luz.color = colorNormal;
                }
            }
        }
    }

    private IEnumerator EfectoFlickerIndividual(Light luz)
    {
        AudioSource audioFoco = luz.GetComponent<AudioSource>();

        while (efectoActivo)
        {
            // 1. Apaga la luz
            luz.enabled = false;
            yield return new WaitForSeconds(Random.Range(minTiempoApagada, maxTiempoApagada));

            // 2. Prende la luz y tira la moneda para el color
            luz.enabled = true;

            // Random.value da un número entre 0 y 1. Si es mayor a 0.5, es rojo. Si no, blanco.
            luz.color = Random.value > 0.5f ? colorAlerta : colorNormal;

            // 3. Audio de la chispa
            if (audioFoco != null && clipChispa != null)
            {
                audioFoco.pitch = Random.Range(0.85f, 1.15f);
                audioFoco.PlayOneShot(clipChispa);
            }

            yield return new WaitForSeconds(Random.Range(minTiempoPrendida, maxTiempoPrendida));
        }
    }
}