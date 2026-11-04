using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Configuración de Elevación")]
    [Tooltip("Tiempo de suavizado para el movimiento vertical de la cámara.")]
    [SerializeField] private float smoothTime = 0.4f;

    [Tooltip("Porcentaje de la altura de pantalla a partir del cual sube la cámara (0.5 = centro).")]
    [SerializeField] private float screenThresholdRatio = 0.5f;

    private Camera cam;
    private float targetY;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        targetY = transform.position.y;
    }

    public void CheckBlockHeight(float blockY, out float cameraDeltaY)
    {
        cameraDeltaY = 0f;
        float camHeight = cam.orthographicSize * 2f;
        float camBottom = transform.position.y - cam.orthographicSize;
        float thresholdY = camBottom + (camHeight * screenThresholdRatio);

        // Si el bloque supera el umbral central, subimos la meta de la cámara
        if (blockY > thresholdY)
        {
            float difference = blockY - thresholdY;
            targetY += difference;
            cameraDeltaY = difference;
        }
    }

    private void LateUpdate()
    {
        // Interpolación suave del eje Y
        Vector3 targetPos = new Vector3(transform.position.x, targetY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}