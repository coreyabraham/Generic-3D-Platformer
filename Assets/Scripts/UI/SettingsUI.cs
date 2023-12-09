using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [field: Header("Frames and Buttons")]
    [field: SerializeField] public Button AudioBtn { get; set; }
    [field: SerializeField] public Button VideoBtn { get; set; }

    [field: SerializeField] public GameObject AudioFrame { get; set; }
    [field: SerializeField] public GameObject VideoFrame { get; set; }

    public void ChangeSubFrame(bool isAudio)
    {
        GameObject target1 = isAudio ? AudioFrame : VideoFrame;
        GameObject target2 = !isAudio ? AudioFrame : VideoFrame;

        target1.SetActive(true);
        target2.SetActive(false);
    }

    private void Start()
    {
        if (!AudioFrame.activeSelf)
            AudioFrame.SetActive(true);

        if (VideoFrame.activeSelf)
            VideoFrame.SetActive(false);
        
        AudioBtn.onClick.AddListener(delegate { ChangeSubFrame(true); });
        AudioBtn.onClick.AddListener(delegate { ChangeSubFrame(false); });
    }
}
