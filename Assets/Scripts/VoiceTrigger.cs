using UnityEngine;

public class VoiceTrigger : MonoBehaviour
{
    [Header("Configuración del Audio")]
    public AudioClip lineaDeVoz;

    public bool destruirAlSonar = true;

    private AudioSource walkieTalkieSource;

    private void Start()
    {
        // Busca un objeto hijo del Player llamado "Audio Radio"
        // Asegurate de que el nombre coincida exactamente
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Transform walkieTransform = player.transform.Find("Audio Radio");
            if (walkieTransform != null)
            {
                walkieTalkieSource = walkieTransform.GetComponent<AudioSource>();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && lineaDeVoz != null && walkieTalkieSource != null)
        {
            walkieTalkieSource.PlayOneShot(lineaDeVoz);

            if (destruirAlSonar)
            {
                Destroy(gameObject);
            }
        }
    }
}