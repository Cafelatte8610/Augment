using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MicSpectrumAnalyz : MonoBehaviour
{
    public int len_spectrum;
    public float[] WaveformData;
    public float[] FreqComponents;
    public float[] keys;
    private float[] key_freq =  {
27.500f,    // 最低音（A0）
29.135f,
30.868f,
32.703f,
34.648f,
36.708f,
38.891f,
41.203f,
43.654f,
46.249f,
48.999f,
51.913f,
55.000f,
58.270f,
61.735f,
65.406f,
69.296f,
73.416f,
77.782f,
82.407f,
87.307f,
92.499f,
97.999f,
103.826f,
110.000f,
116.541f,
123.471f,
130.813f,
138.591f,
146.832f,
155.563f,
164.814f,
174.614f,
184.997f,
195.998f,
207.652f,
220.000f,
233.082f,
246.942f,
261.626f,
277.183f,
293.665f,
311.127f,
329.628f,
349.228f,
369.994f,
391.995f,
415.305f,
440.000f,
466.164f,
493.883f,
523.251f,
554.365f,
587.330f,
622.254f,
659.255f,
698.456f,
739.989f,
783.991f,
830.609f,
880.000f,
932.328f,
987.767f,
1046.502f,
1108.731f,
1174.659f,
1244.508f,
1318.510f,
1396.913f,
1479.978f,
1567.982f,
1661.219f,
1760.000f,
1864.655f,
1975.533f,
2093.005f,
2217.461f,
2349.318f,
2489.016f,
2637.020f,
2793.826f,
2959.955f,
3135.963f,
3322.438f,
3520.000f,
3729.310f,
3951.066f,
4186.009f,  // 最高音（C8）
        };
    public bool[] key_judg;
    public AudioSource Audiosource;
    public int lengthSec=10;
    void Start()
    {
        Audiosource = GetComponent<AudioSource>();
        Debug.Log(AudioSettings.outputSampleRate);
        Audiosource.loop = true;
        Audiosource.clip = Microphone.Start(null, true, lengthSec, 44100);
        while (!(Microphone.GetPosition(null) > 0)) { }
        Audiosource.Play();
    }

    // public Text text;
    string[] noteNames = { "ド", "ド♯", "レ", "レ♯", "ミ", "ファ", "ファ♯", "ソ", "ソ♯", "ラ", "ラ♯", "シ" };

    void Update()
    {
        WaveformData = new float[len_spectrum];
        FreqComponents = new float[len_spectrum];
        key_judg = new bool[88];
        Audiosource.GetOutputData(WaveformData, 0);
        Audiosource.GetSpectrumData(FreqComponents,0,FFTWindow.BlackmanHarris);
        keys = new float[88];
        for(int key = 0; key < 88; ++key) {
            float hz = key_freq[key];
            int m = (int)(AudioSettings.outputSampleRate / hz);
            int N = WaveformData.Length - m;
            float diff = 0f;
            for (int n = 0; n < N; ++n) {
                diff += Mathf.Abs(WaveformData[n] - WaveformData[n + m]);
            }
            diff *= 1f / N;
            keys[key] = diff;
        }

        // string s="";
        for(int i=0;i<10;i++){
            int Ind=System.Array.IndexOf(keys,keys.Min());
            if(FreqComponents[Ind]>=0.0005f){
                key_judg[Ind]=true;

                // s += noteNames[(Ind+21)%12];
                // s +=" , ";
            }

            
            // text.text=s;
        
        
        }
        // m_source.GetSpectrumData(currentValues, 0, FFTWindow.BlackmanHarris);
        // int levelCount = currentValues.Length / 8;

        // var plaing_spectrum = new List<double>{};
        // for (int i =1; i < levelCount-1; i++)
        // {


        //     if (currentValues[i] > 0.005 && ((currentValues[i] > currentValues[i - 1]) && (currentValues[i] < currentValues[i + 1])))
        //     {
                
        //     }
        //     else{
        //         currentValues[i]=0;
        //     }
        // }

        // // string s = " ";

        // for(int i=0;i<10;i++){
        //     int Ind=System.Array.IndexOf(currentValues,currentValues.Max());
        //     plaing_spectrum.Add(tr_freq(Ind));
        //     // s+=tr_freq(Ind).ToString();
        //     // s+=" , ";
        //     currentValues[Ind]=0;
        // }




        // // if(s!=" ")Debug.Log(s); 


        // //鍵盤で全探索
        // for (int i = 0; i < 88; i++)
        // {
        //     if (BinarySearch(plaing_spectrum, key_freq[i]))
        //     {
        //         key_judg[i] = true;
        //     }
        //     else
        //     {
        //         key_judg[i] = false;
        //     }
        // }

        // if(Input.GetKey("space")){
        //     for (int i = 0; i < 88; i++){
        //         key_judg[i]=true;
        //     }
        // }
        
        // フルコンボテスト用
        // for (int i = 0; i < 88; i++)
        // {
        //     key_judg[i]=true;
        // }
    }

    // public double tr_freq(int index)
    // {
    //     return (index * 48000 * 0.5 / currentValues.Length);
    // }
    
    // public bool BinarySearch(List<double> array, int target) //二分探索
    // {
    //     var left = 0;
    //     var right = array.Count - 1;
    //     while (left <= right)
    //     {
    //         var mid = left + (right - left) / 2;
    //         double mid_spe = array[mid];
    //         if (mid_spe + 5.0 >= target && mid_spe - 5.0 <= target) //誤差を考慮して±5の範囲で判定
    //         {
    //             return true;
    //         }
    //         else if (mid_spe < target)
    //         {
    //             left = mid + 1;
    //         }
    //         else
    //         {
    //             right = mid - 1;
    //         }
    //     }
    //     return false;
    // }
}
