using UnityEngine;

public class MascotController : MonoBehaviour
{
    public Transform targetA;
    public Transform targetB;
    public Transform centerEyeAnchor;
    public float speed = 2.0f;
    public float rotationSpeed = 5.0f;
    public float orbitRadius = 0.25f;
    public float orbitSpeed = 0.5f;
    public float eyeOffset = 0.1f;
    public AudioSource audioSource;

    private bool movingToB = true;
    private int reachedBCount = 0;
    private bool isOrbiting = false;
    private float orbitAngle = 0f;
    private Vector3 orbitStartPos;

    void Update()
    {
        if (isOrbiting)
        {
            MoveInCircleAroundB();
            return;
        }

        Vector3 startPos = targetA.position;
        Vector3 endPos = targetB.position;
        float eyeHeight = centerEyeAnchor.position.y - eyeOffset;
        startPos.y = endPos.y = eyeHeight;

        Vector3 targetPos = movingToB ? endPos : startPos;
        Vector3 direction = (targetPos - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, targetPos);

        if (movingToB && distanceToTarget <= 0.5f)
        {
            movingToB = false;
            reachedBCount++;

            if (reachedBCount == 1 && audioSource != null)
            {
                audioSource.Play();
            }

            if (reachedBCount >= 2)
            {
                StartOrbiting();
                return;
            }
        }
        else if (!movingToB && distanceToTarget <= 0.1f)
        {
            movingToB = true;
        }

        Vector3 nextPos = transform.position + direction * speed * Time.deltaTime;
        nextPos.y = eyeHeight;
        transform.position = nextPos;

        if (direction != Vector3.zero)
        {
            // INVERTIERT: Damit das Maskottchen „vorwärts“ fliegt
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void StartOrbiting()
    {
        isOrbiting = true;
        orbitAngle = Mathf.Atan2(transform.position.z - targetB.position.z, transform.position.x - targetB.position.x);
        orbitStartPos = transform.position;
    }

    void MoveInCircleAroundB()
    {
        orbitAngle -= orbitSpeed * Time.deltaTime;

        float x = targetB.position.x + Mathf.Cos(orbitAngle) * orbitRadius;
        float z = targetB.position.z + Mathf.Sin(orbitAngle) * orbitRadius;
        float y = centerEyeAnchor.position.y - eyeOffset;

        Vector3 targetOrbitPos = new Vector3(x, y, z);
        transform.position = Vector3.Lerp(transform.position, targetOrbitPos, Time.deltaTime * 2f);

        Vector3 lookDir = new Vector3(Mathf.Cos(orbitAngle + Mathf.PI / 2), 0, Mathf.Sin(orbitAngle + Mathf.PI / 2));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * rotationSpeed);
    }
}
