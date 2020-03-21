using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MicSpectrumAnalyz : MonoBehaviour
{
    public int SampleNum = 1024; // サンプリング数は2のN乗
    AudioSource m_source;
    float[] currentValues;
    int minFreq, maxFreq;

    private int[] key_freq = { 27, 29, 30, 32, 34, 36, 38, 41, 43, 46, 48, 51, 55, 58, 61, 65, 69, 73, 77, 82, 87, 92, 97, 103, 110, 116, 123, 130, 138, 146, 155, 164, 174, 184, 195, 207, 220, 233, 247, 261, 277, 293, 311, 329, 349, 369, 391, 415, 440, 466, 493, 523, 554, 587, 522, 659, 698, 739, 783, 830, 880, 932, 987, 1046, 1108, 1174, 1244, 1318, 1396, 1479, 1567, 1661, 1760, 1864, 1975, 2093, 2217, 2349, 2489, 2637, 2793, 2959, 3135, 3322, 3520, 3729, 3951, 4186 };
    public bool[] key_judg = new bool[88];
    void Start()
    {
        m_source = GetComponent<AudioSource>();


        currentValues = new float[SampleNum];
         
        if ((m_source != null) && (Microphone.devices.Length > 0))
        {
            string devName = Microphone.devices[0];

            Microphone.GetDeviceCaps(devName, out minFreq, out maxFreq); // 最大最小サンプリング数を得る
            // Debug.Log(minFreq+" "+maxFreq);
            int ms = minFreq / SampleNum; // サンプリング時間を取る
            // Debug.Log(ms);
            m_source.loop = true; // ループ
            m_source.clip = Microphone.Start(devName, true, ms, minFreq); // clipをマイクに設定
            while (!(Microphone.GetPosition(devName) > 0)) { } // きちんと値をとるために待つ
            Microphone.GetPosition(null);
            Debug.Log("GameStart");
            m_source.Play();
        }
    }

    void Update()
    {
        m_source.GetSpectrumData(currentValues, 0, FFTWindow.Hamming);
        int levelCount = currentValues.Length / 8;

        var plaing_spectrum = new List<double>{};
        for (int i =1; i < levelCount-1; i++)
        {


            if (currentValues[i] > 0.001 && ((currentValues[i] > currentValues[i - 1]) && (currentValues[i] < currentValues[i + 1])))
            {
                
            }
            else{
                currentValues[i]=0;
            }
        }

        // string s = " ";

        for(int i=0;i<5;i++){
            int Ind=System.Array.IndexOf(currentValues,currentValues.Max());
            plaing_spectrum.Add(tr_freq(Ind));
            // s+=tr_freq(Ind).ToString();
            // s+=" , ";
            currentValues[Ind]=0;
        }


        // if(s!=" ")Debug.Log(s);


        //鍵盤で全探索
        for (int i = 0; i < 88; i++)
        {
            if (BinarySearch(plaing_spectrum, key_freq[i]))
            {
                key_judg[i] = true;
            }
            else
            {
                key_judg[i] = false;
            }
        }

        // フルコンボテスト用
        // for (int i = 0; i < 88; i++)
        // {
        //     key_judg[i]=true;
        // }
    }

    public double tr_freq(int index)
    {
        return (index * 48000 * 0.5 / currentValues.Length);
    }
    
    public bool BinarySearch(List<double> array, int target) //二分探索
    {
        var left = 0;
        var right = array.Count - 1;
        while (left <= right)
        {
            var mid = left + (right - left) / 2;
            double mid_spe = array[mid];
            if (mid_spe + 5.0 >= target && mid_spe - 5.0 <= target) //誤差を考慮して±5の範囲で判定
            {
                return true;
            }
            else if (mid_spe < target)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }
        return false;
    }
}
