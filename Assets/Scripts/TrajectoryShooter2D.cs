using UnityEngine;
using UnityEngine.InputSystem;

public class TrajectoryShooter2D : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject projectilePrefab;
    public LineRenderer lineRenderer;
    public Camera mainCamera;

    public float shootForce = 10f;
    public int resolution = 30;
    public float spacing = 0.1f;

    private Vector2 shootDirection;
    private Vector2[] points;

    public void OnAim(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        shootDirection = (worldPos - (Vector2)shootPoint.position).normalized;

        DrawTrajectory();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = shootDirection * shootForce;
    }

    void DrawTrajectory()
    {
        points = new Vector2[resolution];
        Vector2 velocity = shootDirection * shootForce;
        Vector2 pos = shootPoint.position;

        for (int i = 0; i < resolution; i++)
        {
            float t = i * spacing;
            Vector2 point = pos + velocity * t + 0.5f * Physics2D.gravity * t * t;
            points[i] = point;
        }

        lineRenderer.positionCount = resolution;
        for (int i = 0; i < resolution; i++)
            lineRenderer.SetPosition(i, points[i]);
    }
}
