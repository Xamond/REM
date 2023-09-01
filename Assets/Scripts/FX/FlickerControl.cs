using System.Collections;
using UnityEngine;

public class FlickerControl : MonoBehaviour
{
    [SerializeField]
    private float minDelay = 0.2f;
    [SerializeField]
    private float maxDelay = 1f;

    private bool isFlickering = false;

    private void Update()
    {
        HandleFlicker();
    }

    /// <summary>
    /// Handles the flickering of the light component.
    /// </summary>
    private void HandleFlicker()
    {
        if (!isFlickering)
        {
            StartCoroutine(FlickeringLight());
        }
    }

    /// <summary>
    /// Coroutine that toggles the light component on and off with a random delay.
    /// </summary>
    private IEnumerator FlickeringLight()
    {
        isFlickering = true;

        var lightComponent = GetComponent<Light>();

        lightComponent.enabled = false;
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        lightComponent.enabled = true;
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

        isFlickering = false;
    }
}
