using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public Transform cameraTrnasform;
    public Transform followTrnasform;
    public Vector3 followDirection { get; set; }    
    [SerializeField]
    private Vector3 currentFollowPosition;
    [SerializeField]
    private Vector3 followPointFraming = new Vector3(0f, 0f);
    private float followingSharpness = 10000f;

    // Distance 카메라 거리조절    
    private float followDistance { get; set; }
    [SerializeField]
    private float currentDistance;
    private float minDistance = 0f;
    private float maxDistance = 10f;
    private float distanceMovementSpeed = 5f;
    private float distanceMovementSharpness = 10f;

    // Angle 카메라 앵글
    private float cameraVerticalAngle = 0;
    [Range(-90f, 90f)]
    private float minVerticalAngle = -90f;
    [Range(-90f, 90f)]
    private float maxVerticalAngle = 90f;
    private float angleRotationSpeed = 1f;
    private float angleRotationSharpness = 10000f;

    // Obstruction
    private bool obstructedEnable;
    private float obstructionCheckRadius = 0.2f;
    private LayerMask obstructionLayers = 6;
    private float obstructionSharpness = 10000f;
    [SerializeField]
    private List<Collider> ignoredColliders = new List<Collider>();                    
    private RaycastHit[] obstructions = new RaycastHit[maxObstructions];
    private const int maxObstructions = 32;

    void Start()
    {
        CameraInit();
    }

    void LateUpdate()
    {
        CameraHandle();
    }

    private void CameraInit()
    {
        cameraTrnasform = Camera.main.transform;
        followTrnasform = gameObject.transform;
        followDistance = currentDistance;
        followDirection = followTrnasform.forward;
        currentFollowPosition = followTrnasform.position;
    }

    private void CameraHandle()
    {
        float mouseY = Input.GetAxisRaw("Mouse Y");
        float mouseX = Input.GetAxisRaw("Mouse X");
        Vector3 inputVector = new Vector3(mouseX, mouseY, 0f);
        float inputScroll = -Input.GetAxis("Mouse ScrollWheel");
        UpdateCamera(Time.deltaTime, inputScroll, inputVector);
    }

    private void UpdateCamera(float deltaTime, float inputScroll, Vector3 inputVector)
    {
        if (followTrnasform)
        {
            // Process rotation input
            Quaternion rotationFromInput = Quaternion.Euler(followTrnasform.up * (inputVector.x * angleRotationSpeed));
            followDirection = rotationFromInput * followDirection;
            followDirection = Vector3.Cross(followTrnasform.up, Vector3.Cross(followDirection, followTrnasform.up));
            Quaternion planarRot = Quaternion.LookRotation(followDirection, followTrnasform.up);

            cameraVerticalAngle -= (inputVector.y * angleRotationSpeed);
            cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, minVerticalAngle, maxVerticalAngle);
            Quaternion verticalRot = Quaternion.Euler(cameraVerticalAngle, 0, 0);
            Quaternion targetRotation = Quaternion.Slerp(cameraTrnasform.rotation, planarRot * verticalRot, 1f - Mathf.Exp(-angleRotationSharpness * deltaTime));

            // Apply rotation
            cameraTrnasform.rotation = targetRotation;

            // Process distance input
            if (obstructedEnable && Mathf.Abs(inputScroll) > 0f)
            {
                followDistance = currentDistance;
            }
            followDistance += inputScroll * distanceMovementSpeed;
            followDistance = Mathf.Clamp(followDistance, minDistance, maxDistance);

            // Find the smoothed follow position
            currentFollowPosition = Vector3.Lerp(currentFollowPosition, followTrnasform.position, 1f - Mathf.Exp(-followingSharpness * deltaTime));

            // Handle obstructions
            {
                RaycastHit closestHit = new RaycastHit();
                closestHit.distance = Mathf.Infinity;
                int obstructionCount = Physics.SphereCastNonAlloc(currentFollowPosition, obstructionCheckRadius, -cameraTrnasform.forward, obstructions, followDistance, obstructionLayers, QueryTriggerInteraction.Ignore);
                for (int i = 0; i < obstructionCount; i++)
                {
                    bool isIgnored = false;
                    for (int j = 0; j < ignoredColliders.Count; j++)
                    {
                        if (ignoredColliders[j] == obstructions[i].collider)
                        {
                            isIgnored = true;
                            break;
                        }
                    }
                    for (int j = 0; j < ignoredColliders.Count; j++)
                    {
                        if (ignoredColliders[j] == obstructions[i].collider)
                        {
                            isIgnored = true;
                            break;
                        }
                    }

                    if (!isIgnored && obstructions[i].distance < closestHit.distance && obstructions[i].distance > 0)
                    {
                        closestHit = obstructions[i];
                    }
                }

                // If obstructions detecter
                if (closestHit.distance < Mathf.Infinity)
                {
                    obstructedEnable = true;
                    currentDistance = Mathf.Lerp(currentDistance, closestHit.distance, 1 - Mathf.Exp(-obstructionSharpness * deltaTime));
                }
                // If no obstruction
                else
                {
                    obstructedEnable = false;
                    currentDistance = Mathf.Lerp(currentDistance, followDistance, 1 - Mathf.Exp(distanceMovementSharpness * deltaTime));
                }
            }

            // Find the smoothed camera orbit position
            Vector3 targetPosition = currentFollowPosition - ((targetRotation * Vector3.forward) * currentDistance);

            // Handle framing
            targetPosition += cameraTrnasform.right * followPointFraming.x;
            targetPosition += cameraTrnasform.up * followPointFraming.y;

            // Apply position
            cameraTrnasform.position = targetPosition;
        }
    }
}

