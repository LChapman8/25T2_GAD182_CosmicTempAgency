using UnityEngine;
using TMPro;
using System.Collections;

public class WorldCreditsTrigger : MonoBehaviour
{
    [Header("Credits Settings")]
    public TextMeshPro creditsTextPrefab;   // Prefab of 3D TextMeshPro
    public Transform triggerPosition;       // Optional starting position (if null, uses trigger)
    [TextArea(5, 20)]
    public string[] creditsLines;           // Lines of the credits
    public float lineSpacing = 2f;          // Space between lines
    public float scrollSpeed = 2f;          // Units per second
    public float totalScrollHeight = 50f;   // How far the credits scroll before ending
    public float spawnDelay = 0.3f;         // Delay between spawning each line

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(SpawnAndScrollCredits());
        }
    }

    IEnumerator SpawnAndScrollCredits()
    {
        Vector3 spawnPos = triggerPosition != null ? triggerPosition.position : transform.position;

        // Array to hold spawned text objects
        TextMeshPro[] spawnedTexts = new TextMeshPro[creditsLines.Length];

        for (int i = 0; i < creditsLines.Length; i++)
        {
            TextMeshPro textInstance = Instantiate(creditsTextPrefab, spawnPos, Quaternion.identity);
            textInstance.text = creditsLines[i];

            // Face the camera upright
            Vector3 dir = Camera.main.transform.position - textInstance.transform.position;
            dir.y = 0; // lock vertical tilt
            textInstance.transform.rotation = Quaternion.LookRotation(-dir);

            spawnedTexts[i] = textInstance;

            // Move spawn position downward for next line
            spawnPos.y -= lineSpacing;

            // Optional: wait a little before spawning next line
            yield return new WaitForSeconds(spawnDelay);
        }

        // Scroll all lines upward over time
        float scrolled = 0f;
        while (scrolled < totalScrollHeight)
        {
            foreach (TextMeshPro t in spawnedTexts)
            {
                if (t != null)
                    t.transform.position += Vector3.up * scrollSpeed * Time.deltaTime;
            }

            scrolled += scrollSpeed * Time.deltaTime;
            yield return null;
        }

        // Optional: destroy spawned texts after scroll
        foreach (TextMeshPro t in spawnedTexts)
        {
            if (t != null)
                Destroy(t.gameObject);
        }
    }
}
