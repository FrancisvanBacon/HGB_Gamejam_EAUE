using System.Collections;
using System.Collections.Generic;
using Actors.Player;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CameraTargetGroupController : MonoBehaviour {

    [SerializeField] private List<CharacterStateController> assignedCharacters;
    [SerializeField] private float cameraSpeed = 1f;
    [SerializeField] private CinemachineVirtualCamera dynamicCamera;
    [SerializeField] private CinemachineVirtualCamera staticCamera;
    
    private CinemachineVirtualCamera m_currentCamera;
    
    private CinemachineTargetGroup m_targetGroup;

    private Dictionary<CharacterStateController, int> m_targetLookup = new Dictionary<CharacterStateController, int>();

    private IEnumerator m_zoomRoutine;

    private void Start() {
        m_targetGroup = GetComponent<CinemachineTargetGroup>();

        foreach (var character in assignedCharacters) {
            
            m_targetLookup.Add(character, m_targetGroup.FindMember(character.transform));
            
        }

        m_currentCamera = dynamicCamera;
        m_currentCamera.Priority = 11;
        staticCamera.Priority = 10;

        var inputManager = GameObject.FindFirstObjectByType<PlayerInputManager>();

        if (inputManager != null) {
            inputManager.onPlayerJoined += input => {
                SwitchToStaticCamera(true);
            };
        }
    }

    public void SwitchCameraViews(InputAction.CallbackContext context) {

        if (context.performed) {
        
            Debug.Log("Performed");
            int prio1 = dynamicCamera.Priority;
            int prio2 = staticCamera.Priority;

            dynamicCamera.Priority = prio2;
            staticCamera.Priority = prio1;
            
            m_currentCamera = dynamicCamera.Priority > staticCamera.Priority ? dynamicCamera : staticCamera;
        }
    }

    public void SwitchToStaticCamera(bool enable) {
        m_currentCamera = staticCamera;
        staticCamera.Priority = enable ? 11 : 10;
        dynamicCamera.Priority = enable ? 10 : 11;
    } 

    public void ZoomCameraIn(InputAction.CallbackContext context) {
        if (context.performed) {
            ZoomCameras(true, true);
        } else if (context.canceled) {
            ZoomCameras(false, true);
        }
    }
    
    public void ZoomCameraOut(InputAction.CallbackContext context) {
        if (context.performed) {
            ZoomCameras(true, false);
        } else if (context.canceled) {
            ZoomCameras(false, false);
        }
    }

    public void ZoomCameras(bool start, bool isZoomIn) {
        
        if (start && m_zoomRoutine == null) {
            
            m_zoomRoutine = ZoomCamerasRoutine(isZoomIn);
            StartCoroutine(m_zoomRoutine);

        } else if (!start && m_zoomRoutine != null) {
            
            StopCoroutine(m_zoomRoutine);
            m_zoomRoutine = null;
        }
    }

    public void OnSwitchAdjustCamera() {
        
        List<CharacterStateController> activeCharacters = new List<CharacterStateController>();

        foreach (var character in assignedCharacters) {

            if (character.IsPlayerControlled) {
                activeCharacters.Add(character);
            }
        }
        
        foreach (var character in assignedCharacters) {
            StartCoroutine(character.IsPlayerControlled
                ? AdjustWeightRoutine(m_targetLookup[character], 1f / activeCharacters.Count)
                : AdjustWeightRoutine(m_targetLookup[character], 0));
        }

    }


    private IEnumerator AdjustWeightRoutine(int targetIndex, float toWeight) {
        float t = 0f;
        float fromWeight = m_targetGroup.m_Targets[targetIndex].weight;
        
        while (Mathf.Abs(m_targetGroup.m_Targets[targetIndex].weight - toWeight) > 0.001f) {
            
            m_targetGroup.m_Targets[targetIndex].weight = Mathf.Lerp(fromWeight, toWeight, t);
            m_targetGroup.DoUpdate();
            t += Time.deltaTime * cameraSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return null;
    }

    private IEnumerator ZoomCamerasRoutine(bool zoomIn) {

        var offset = m_currentCamera.GetComponent<CinemachineCameraOffset>();
        float factor = zoomIn ? 1f : -1f;

        bool canZoom = zoomIn ? offset.m_Offset.z < -5f : offset.m_Offset.z > -15f;
        
        while (canZoom) {
            float zOffset = offset.m_Offset.z + Time.deltaTime * factor * cameraSpeed;
           offset.m_Offset.z = zOffset;
           
           canZoom = zoomIn ? offset.m_Offset.z < -5f : offset.m_Offset.z > -15f;
           
           yield return null;
        }
    }
}