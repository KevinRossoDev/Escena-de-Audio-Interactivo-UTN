using UnityEngine;

public class EventoSightOrSound : MonoBehaviour
{
    public AudioSource fuenteDeSonido;

    [Header("Configuración del Evento")]
    public bool sonarSoloUnaVez = true;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (fuenteDeSonido != null)
            {
                fuenteDeSonido.Play(); 
            }
            if (sonarSoloUnaVez)
            {
                gameObject.SetActive(false);
            }
        }
    }
}