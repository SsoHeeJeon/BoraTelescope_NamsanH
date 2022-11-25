using UnityEngine;

namespace KinematicCharacterController.Examples
{
    public class MetaLivePlayer : MonoBehaviour
    {
        public Transform cameraTarget;
        public MetaLiveCamera CharacterCamera;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";

        private void Start()
        {
            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(cameraTarget);
            //CharacterCamera.SetFollowTransform(tt.transform);

            // Ignore the character's collider(s) for camera obstruction checks
            //CharacterCamera.IgnoredColliders.Clear();
            //CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }

        private void LateUpdate()
        {
            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
            float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);
            float scrollInput = -Input.GetAxis(MouseScrollInput);
            CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);
        }
    }
}