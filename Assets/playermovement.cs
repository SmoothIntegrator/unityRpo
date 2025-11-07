using UnityEngine;
using System.Collections;

public class DelayedLevelTeleporter2D : MonoBehaviour
{
    [Header("Level Settings")]
    public Transform[] levels;        // All possible level positions
    public int currentLevel = 0;      // Starting level index

    [Header("Timing Settings")]
    public float moveDelay = 2f;      // Delay before teleport happens
    public float moveCooldown = 1f;   // Cooldown after teleport

    private bool canMove = true;      // Can the player press input
    private Coroutine moveRoutine;    // To prevent overlapping coroutines

    void Start()
    {
        // Ensure we start at a valid level
        if (levels != null && levels.Length > 0)
        {
            currentLevel = Mathf.Clamp(currentLevel, 0, levels.Length - 1);
            transform.position = new Vector2(levels[currentLevel].position.x, levels[currentLevel].position.y);
        }
        else
        {
            Debug.LogWarning("⚠️ No level Transforms assigned in DelayedLevelTeleporter2D!");
        }
    }

    void Update()
    {
        if (!canMove || levels.Length == 0)
            return;

        // Move up
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveRoutine = StartCoroutine(MoveAfterDelay(currentLevel + 1));
        }
        // Move down
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveRoutine = StartCoroutine(MoveAfterDelay(currentLevel - 1));
        }
    }

    IEnumerator MoveAfterDelay(int targetLevel)
    {
        canMove = false; // Disable input immediately

        // Wait before teleporting
        yield return new WaitForSeconds(moveDelay);

        // Clamp the level index so it doesn't go out of range
        targetLevel = Mathf.Clamp(targetLevel, 0, levels.Length - 1);

        // Teleport if a new level
        if (targetLevel != currentLevel)
        {
            Vector2 newPos = new Vector2(levels[targetLevel].position.x, levels[targetLevel].position.y);
            transform.position = newPos;
            currentLevel = targetLevel;
        }

        // Wait for cooldown after teleport
        yield return new WaitForSeconds(moveCooldown);

        canMove = true;
        moveRoutine = null;
    }
}
