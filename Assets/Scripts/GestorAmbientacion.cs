using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(CharacterController))]
public class GestorAmbientacion : MonoBehaviour
{
    [Header("Snapshots del Mixer")]
    public AudioMixerSnapshot snapCorredor;
    public AudioMixerSnapshot snapAuditorio;
    public AudioMixerSnapshot snapTension;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        float velocidadActual = controller.velocity.magnitude;
        float tiempoTransicion = (velocidadActual > 3f) ? 0.5f : 4.0f;


        if (other.CompareTag("ZonaCorredor"))
        {
            snapCorredor.TransitionTo(tiempoTransicion);
        }
        else if (other.CompareTag("ZonaAuditorio"))
        {
            snapAuditorio.TransitionTo(tiempoTransicion);
        }
        else if (other.CompareTag("ZonaTension"))
        {
            snapTension.TransitionTo(tiempoTransicion);
        }
    }
}