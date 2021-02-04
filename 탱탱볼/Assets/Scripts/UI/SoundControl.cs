using UnityEngine.UI;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    [SerializeField] private Sprite Btn_Sound;
    [SerializeField] private Sprite Btn_Mute;
    [SerializeField] private GameObject Btn_Handle;
    private GameManager GM;
    private AudioSource AM;
    private Slider S;

    private void Awake()
    {
        S = GetComponent<Slider>();
        GameObject obj = GameObject.Find("GameManager");
        GM = obj.GetComponent<GameManager>();
        AM = obj.GetComponent<AudioSource>();

        S.value = GM.fVolume;
        CheckVol();
    }

    public void VolumeControl()
    {
        GM.fVolume = S.value;
        AM.volume = GM.fVolume;
        CheckVol();
    }

    private void CheckVol()
    {
        if (S.value == 0f) Btn_Handle.GetComponent<Image>().sprite = Btn_Mute;
        else Btn_Handle.GetComponent<Image>().sprite = Btn_Sound;
    }
}
