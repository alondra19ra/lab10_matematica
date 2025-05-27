using UnityEngine;
using UnityEngine.InputSystem;

public class TrajectoryShooter : MonoBehaviour
{

    public Camera mainCamera;
    public Transform shootPoint;
    public GameObject projectilePrefab;
    public LineRenderer lineRenderer;

    public int trajectoryPointsCount = 30;
    public float timeBetweenPoints = 0.1f;
    public float shootPower = 15f;

    private Vector3 targetDirection;
    private Vector3[] trajectoryPoints;

    public void OnAim(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 targetPoint = ray.origin + ray.direction * 30f;

        targetDirection = (targetPoint - shootPoint.position).normalized;

        CalculateTrajectory();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (projectilePrefab == null || trajectoryPoints == null) return;

        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.linearVelocity = targetDirection * shootPower;
    }

    void CalculateTrajectory()
    {
        trajectoryPoints = new Vector3[trajectoryPointsCount];
        Vector3 velocity = targetDirection * shootPower;
        Vector3 startPoint = shootPoint.position;

        for (int i = 0; i < trajectoryPointsCount; i++)
        {
            float t = i * timeBetweenPoints;
            Vector3 position = startPoint + velocity * t + 0.5f * Physics.gravity * t * t;
            trajectoryPoints[i] = position;
        }

        lineRenderer.positionCount = trajectoryPointsCount;
        lineRenderer.SetPositions(trajectoryPoints);
    }
}
