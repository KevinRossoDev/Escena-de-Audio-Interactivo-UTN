using UnityEngine;
using UnityEngine.Audio;

public class AudioSnapshotTrigger : MonoBehaviour
{
    [Header("Configuración del Snapshot")]
    public AudioMixerSnapshot snapshotDestino;

    [Header("Configuración de Transición")]
    public float tiempoDeTransicion = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (snapshotDestino != null)
            {
                snapshotDestino.TransitionTo(tiempoDeTransicion);
                Debug.Log("Transición al snapshot: " + snapshotDestino.name);
            }
        }
    }
}