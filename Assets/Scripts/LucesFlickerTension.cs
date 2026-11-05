using UnityEngine;
using System.Collections;

public class LucesFlickerTension : MonoBehaviour
{
    [Header("Luces a controlar")]
    public Light[] luces;

    [Header("Colores del Parpadeo")]
    public Color colorNormal = Color.white;
   
    public Color colorAlerta = Color.red;

   
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
            luz.enabled = false;
            yield return new WaitForSeconds(Random.Range(minTiempoApagada, maxTiempoApagada));

            luz.enabled = true;

            luz.color = Random.value > 0.5f ? colorAlerta : colorNormal;

            if (audioFoco != null && clipChispa != null)
            {
                audioFoco.pitch = Random.Range(0.85f, 1.15f);
                audioFoco.PlayOneShot(clipChispa);
            }

            yield return new WaitForSeconds(Random.Range(minTiempoPrendida, maxTiempoPrendida));
        }
    }
}