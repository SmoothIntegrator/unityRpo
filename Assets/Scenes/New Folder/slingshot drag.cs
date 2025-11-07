using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class SlingshotDrag : MonoBehaviour
{
    [Header("Slingshot Settings (Auto-Detected)")]
    [HideInInspector] public ProjectileManager projectileManager; // auto-assigned at runtime
    [HideInInspector] public Transform anchorPoint;               // slingshot base point

    [Header("Gameplay Settings")]
    public float snapRadius = 1.5f;
    public float maxDragDistance = 3f;
    public float launchPower = 8f;
    public LayerMask collisionLayers;

    [Header("Visual Settings")]
    public LineRenderer slingshotLine;
    public LineRenderer trajectoryLine;
    public int trajectoryResolution = 20;
    public float timeStep = 0.05f;

    private Rigidbody2D rb;
    private bool isSnapped = false;
    private bool isDragging = false;
    private bool hasLaunched = false;

    void Awake()
    {
        // Automatically find the manager and anchor point if not assigned
        if (projectileManager == null)
            projectileManager = FindObjectOfType<ProjectileManager>();

        if (anchorPoint == null && projectileManager != null)
            anchorPoint = projectileManager.handPosition;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;

        if (slingshotLine != null)
        {
            slingshotLine.positionCount = 2;
            slingshotLine.enabled = false;
        }

        if (trajectoryLine != null)
        {
            trajectoryLine.positionCount = trajectoryResolution;
            trajectoryLine.enabled = false;
        }

        // Snap if near anchor
        if (anchorPoint != null && Vector2.Distance(transform.position, anchorPoint.position) <= snapRadius)
            SnapToAnchor();
    }

    void Update()
    {
        if (anchorPoint == null) return;

        if (!isSnapped && !hasLaunched)
        {
            if (Vector2.Distance(transform.position, anchorPoint.position) <= snapRadius)
                SnapToAnchor();
        }

        if (isSnapped)
            HandleDragAndLaunch();

        if (hasLaunched)
        {
            if (slingshotLine != null) slingshotLine.enabled = false;
            if (trajectoryLine != null) trajectoryLine.enabled = false;
        }
    }

    void SnapToAnchor()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        transform.position = anchorPoint.position;
        isSnapped = true;
    }

    void HandleDragAndLaunch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mousePos, transform.position) < 1f)
                isDragging = true;

            if (slingshotLine != null) slingshotLine.enabled = true;
            if (trajectoryLine != null) trajectoryLine.enabled = true;
        }

        if (isDragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dragDir = mousePos - (Vector2)anchorPoint.position;

            if (dragDir.magnitude > maxDragDistance)
                dragDir = dragDir.normalized * maxDragDistance;

            transform.position = (Vector2)anchorPoint.position + dragDir;
            UpdateVisuals();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            Launch();
        }
    }

    void Launch()
    {
        rb.isKinematic = false;
        Vector2 launchDir = (Vector2)anchorPoint.position - (Vector2)transform.position;
        rb.velocity = launchDir * launchPower;

        isSnapped = false;
        hasLaunched = true;

        if (slingshotLine != null) slingshotLine.enabled = false;
        if (trajectoryLine != null) trajectoryLine.enabled = false;
    }

    void UpdateVisuals()
    {
        if (slingshotLine != null)
        {
            slingshotLine.SetPosition(0, anchorPoint.position);
            slingshotLine.SetPosition(1, transform.position);
        }

        if (trajectoryLine != null)
        {
            Vector2 launchDir = (Vector2)anchorPoint.position - (Vector2)transform.position;
            Vector2 launchVel = launchDir * launchPower;

            List<Vector3> points = new List<Vector3>();
            Vector2 currentPos = transform.position;
            Vector2 currentVel = launchVel;

            for (int i = 0; i < trajectoryResolution; i++)
            {
                Vector2 nextPos = currentPos + currentVel * timeStep +
                                  0.5f * Physics2D.gravity * timeStep * timeStep;
                currentVel += Physics2D.gravity * timeStep;
                points.Add(nextPos);

                RaycastHit2D hit = Physics2D.Linecast(currentPos, nextPos, collisionLayers);
                if (hit.collider != null)
                {
                    points.Add(hit.point);
                    break;
                }

                currentPos = nextPos;
            }

            trajectoryLine.positionCount = points.Count;
            for (int i = 0; i < points.Count; i++)
                trajectoryLine.SetPosition(i, points[i]);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasLaunched)
            Invoke(nameof(DestroyProjectile), 0.4f);
    }

    void DestroyProjectile()
    {
        if (projectileManager == null)
            projectileManager = FindObjectOfType<ProjectileManager>();

        if (projectileManager != null)
            projectileManager.OnProjectileDestroyed();

        Destroy(gameObject);
    }
}