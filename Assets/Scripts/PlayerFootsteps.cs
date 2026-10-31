using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerFootsteps : MonoBehaviour
{
    [Header("Referencias de Audio")]
    public AudioSource sourcePasos;
    public AudioClip[] clipsDePasos;

    [Header("Ritmo y Tiempos")]
    public float tiempoEntrePasos = 0.5f;
    private float temporizador;

    [Header("Variación")]
    public float pitchMinimo = 0.9f;
    public float pitchMaximo = 1.1f;
    public float volumenMinimo = 0.8f;
    public float volumenMaximo = 1.0f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        temporizador = 0f;
    }

    void Update()
    {
        // Calculamos solo la velocidad en X y Z (ignoramos las caídas o saltos en Y)
        Vector3 velocidadHorizontal = new Vector3(controller.velocity.x, 0, controller.velocity.z);

        // Si se mueve y el motor cree que toca el piso
        if (controller.isGrounded && velocidadHorizontal.magnitude > 0.1f)
        {
            temporizador += Time.deltaTime;

            if (temporizador >= tiempoEntrePasos)
            {
                ReproducirPaso();
                temporizador = 0f;
            }
        }
        // SOLO reseteamos el ritmo si el jugador se frena por completo
        else if (velocidadHorizontal.magnitude < 0.1f)
        {
            temporizador = tiempoEntrePasos;
        }
    }

    private void ReproducirPaso()
    {
        if (clipsDePasos.Length == 0 || sourcePasos == null) return;

        
        AudioClip clipAleatorio = clipsDePasos[Random.Range(0, clipsDePasos.Length)];
        sourcePasos.clip = clipAleatorio;

        
        sourcePasos.pitch = Random.Range(pitchMinimo, pitchMaximo);
        sourcePasos.volume = Random.Range(volumenMinimo, volumenMaximo);

        
        sourcePasos.Play();
    }
}