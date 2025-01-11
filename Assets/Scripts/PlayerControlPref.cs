
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControlPref : MonoBehaviour {

    [SerializeField] private Image keyboardButtonImage;
    [SerializeField] private Image gamepadButtonImage;

    private bool m_lockPrefs;

    private void Start() {
        if (PlayerPrefs.HasKey("ControlDevice")) {
            UpdateView(PlayerPrefs.GetString("ControlDevice"));
        }
    }
    public void SetPreferredDevice(string device) {
        PlayerPrefs.SetString("ControlDevice", device);

        UpdateView(device);
    }
    
    public void SetPreferredDevice(PlayerInput input) {

        if (PlayerPrefs.HasKey("ControlDevice")) return;

        PlayerPrefs.SetString("ControlDevice", input.currentControlScheme);

        UpdateView(input.currentControlScheme);
    }

    private void UpdateView(string selectedDevice) {
        
        if (selectedDevice.Equals("Gamepad")) {
            keyboardButtonImage.color = Color.gray;
            gamepadButtonImage.color = Color.white;
        }
        else {
            keyboardButtonImage.color = Color.white;
            gamepadButtonImage.color = Color.gray;
        }
        
    }
    
}

