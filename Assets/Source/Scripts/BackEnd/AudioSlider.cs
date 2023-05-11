using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSlider : MonoBehaviour
{
    public string Param = "master";
    public AudioMixer mixer;
    public Slider slider;
    private void Start()
    {
        //get the last saved audio value
        float value= PlayerPrefs.GetFloat(Param, 0.75f);

        slider.value = value;//set the value of the slder to the audio level
        slider.onValueChanged.AddListener(SetAudioLevel);//set the value changed callback

        SetMixerValue(value);//set the audio level in the mixer
    }


    void SetMixerValue(float Value)
    {
        mixer.SetFloat(Param, Mathf.Log10(Value) * 20);//convert the value to a logarthmic roll off to match the correct values in the mixer
    }
    public void SetAudioLevel(float sliderValue)//slider value changed callback
    {
        SetMixerValue(sliderValue);//set the audio level in the mixer
        PlayerPrefs.SetFloat(Param, sliderValue);//save the value
    }
    
}
