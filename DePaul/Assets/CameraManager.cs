using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float speed;
    public float duration = 1;

    public Vector3 startingPos;
    public Vector3 startingRot;
    private Vector3 endingPos;
    private Vector3 endingRot;

    private void Start()
    {
        transform.position = startingPos;
        transform.rotation = Quaternion.Euler(startingRot);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endingPos, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.rotation.eulerAngles, endingRot, speed * Time.deltaTime));
    }
    
    private IEnumerator TransitionCoroutine(Vector3 startPos, Vector3 endPos, Vector3 startRot, Vector3 endRot, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;

            // Smooth step for easing
            t = t * t * (3f - 2f * t);

            // Lerp position
            transform.position = Vector3.Lerp(startPos, endPos, t);

            // Lerp rotation
            Quaternion startQuaternion = Quaternion.Euler(startRot);
            Quaternion endQuaternion = Quaternion.Euler(endRot);
            transform.rotation = Quaternion.Lerp(startQuaternion, endQuaternion, t);

            yield return null;
        }

        // Ensure the final position and rotation are exactly the target values
        transform.position = endPos;
        transform.rotation = Quaternion.Euler(endRot);
    }

    
    public void SwitchStage(Vector3 position, Vector3 rotation)
    {
        endingPos = position;
        endingRot = rotation;
        StartCoroutine(TransitionCoroutine(transform.position, endingPos, transform.rotation.eulerAngles, endingRot, duration));
    }
    
}
