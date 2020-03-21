using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text;

public class midi_analyz : MonoBehaviour {


public enum NoteType
{
  Normal,      
  LongStart,   
  LongEnd,     
}

public struct NoteData
{
  public int eventTime;  
  public int laneIndex;  
  public NoteType type;  
}

public struct TempoData
{
  public int eventTime;  
  public float bpm;      
  public float tick;     
}



/// ヘッダーチャンク情報を格納する構造体
public struct HeaderChunkData
{
    public byte[] chunkID;     
    public int dataLength;     
    public short format;       
    public short tracks;       
    public short division;     
};

/// トラックチャンク情報を格納する構造体
public struct TrackChunkData
{
    public byte[] chunkID;     
    public int dataLength;     
    public byte[] data;        
};
    
    public bool Ready=false;
    public List<NoteData> noteList = new List<NoteData>();
    public List<TempoData> tempoList = new List<TempoData>();

    public void LoadMSF(string fileAddres){
        noteList.Clear();
        tempoList.Clear();

        using(var stream = new FileStream(fileAddres, FileMode.Open, FileAccess.Read))
        using(var reader = new BinaryReader(stream)){

            var headerCH =new HeaderChunkData();

            // チャンクID
            headerCH.chunkID = reader.ReadBytes(4);


            // 自分のPCがリトルエンディアンならバイト順を逆に
            if (BitConverter.IsLittleEndian)
            {
                // ヘッダ部のデータ長
                var byteArray = reader.ReadBytes(4);
                Array.Reverse(byteArray);
                headerCH.dataLength = BitConverter.ToInt32(byteArray, 0);
                // フォーマット(2byte)
                byteArray = reader.ReadBytes(2);
                Array.Reverse(byteArray);
                headerCH.format = BitConverter.ToInt16(byteArray, 0);
                // トラック数(2byte)
                byteArray = reader.ReadBytes(2);
                Array.Reverse(byteArray);
                headerCH.tracks = BitConverter.ToInt16(byteArray, 0);
                // タイムベース(2byte)
                byteArray = reader.ReadBytes(2);
                Array.Reverse(byteArray);
                headerCH.division = BitConverter.ToInt16(byteArray, 0);
            }
            else
            {
                // ヘッダ部のデータ長
                headerCH.dataLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                // フォーマット(2byte)
                headerCH.format = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                // トラック数(2byte)
                headerCH.tracks = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                // タイムベース(2byte)
                headerCH.division = BitConverter.ToInt16(reader.ReadBytes(2), 0);
            }
            
            // トラックチャンク
            var trackCH = new TrackChunkData[headerCH.tracks];
            
            // トラック数
            for(int i=0;i<headerCH.tracks;i++){

                // チャンクID
                trackCH[i].chunkID = reader.ReadBytes(4);

                // 自分のPCがリトルエンディアンなら変換する
                if (BitConverter.IsLittleEndian)
                {
                    // トラックのデータ長読み込み
                    var byteArray = reader.ReadBytes(4);
                    Array.Reverse(byteArray);
                    trackCH[i].dataLength = BitConverter.ToInt32(byteArray, 0);
                }
                else
                {
                    trackCH[i].dataLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                }

                // データ部読み込み
                trackCH[i].data=reader.ReadBytes(trackCH[i].dataLength);

                // トラックデータ解析へ
                TrackDataAnalys(trackCH[i].data,headerCH);
            }
        }
    }

    void TrackDataAnalys(byte[] data,HeaderChunkData headerCH){
        // Debug.Log(data.Length);
        uint CrTime=0;                    
        byte statusByte=0;                
        bool[] longFlags=new bool[128];   

        for(int i=0;i<data.Length;){

            // Debug.Log(i);
            
            bool Fintrack=false;
            uint deltaTime=0;

            while(true){
                var tmp = data[i++];
                deltaTime |= (tmp & (uint)0x7f);
                if ((tmp & 0x80) == 0) break;
                deltaTime = deltaTime << 7;
            }

            // 現在の時間にデルタタイムを足す
            CrTime+=deltaTime;
            if (data[i] >= 0x80)
            {
                statusByte = data[i++];
            }
            //else:ランニングステータス適応(前回のステータスバイトを使いまわす)
            
            // ステータスバイト後のデータ保存用
            byte dataByte0, dataByte1;
            // byte dataByte2, dataByte3;
            if (statusByte >= 0x80 && statusByte <= 0xef){
                switch (statusByte & 0xf0){
                    /* チャンネルメッセージ */
                    // ノートオフ
                    case 0x80:
                        dataByte0 = data[i++];
                        dataByte1 = data[i++];
                        if (longFlags[dataByte0])
                        {
                            var note = new NoteData();
                            note.eventTime = (int)CrTime;
                            note.laneIndex = (int)dataByte0;
                            note.type = NoteType.LongEnd;

                            // リストにつっこむ
                            noteList.Add(note);

                            longFlags[note.laneIndex] = false;
                        }
                        break;
                     // ノートオン(ノートオフが呼ばれるまでは押しっぱなし扱い)
                    case 0x90:
                            // どのキーが押されたか
                            dataByte0 = data[i++];
                            dataByte1 = data[i++];

                            {
                                // ノート情報生成
                                var note = new NoteData();
                                note.eventTime = (int)CrTime;
                                note.laneIndex = (int)dataByte0;
                                note.type = NoteType.Normal;
                                if (dataByte1 == 127)
                                {
                                   note.type = NoteType.LongStart;
                                   longFlags[note.laneIndex] = true;
                                }
                                // ノートオフイベントではなく、ベロシティ値0をノートオフとして保存する形式もあるので対応
                                if (dataByte1 == 0)
                                {
                                    // 同じレーンで前回がロングノーツ始点なら
                                    if (longFlags[note.laneIndex])
                                    {
                                        note.type = NoteType.LongEnd;
                                        longFlags[note.laneIndex] = false;
                                    }
                                }

                                // リストにつっこむ
                                noteList.Add(note);
                            }
                            break;
                        case 0xa0:
                            i += 2; // 使わないのでスルー
                            break;
                        case 0xb0:
                            dataByte0 = data[i++];
                            dataByte1 = data[i++];

                            // ※0x00-0x77までがコントロールチェンジで、それ以上はチャンネルモードメッセージとして処理する
                            if (dataByte0 < 0x78)
                            {
                                // コントロールチェンジ
                            }
                            else
                            {
                              
                                switch (dataByte0)
                                {
                                    case 0x78: 
                                        break;
                                    case 0x79:
                                        break;
                                    case 0x7a:
                                        break;
                                    case 0x7b:
                                        break;
                                    // オムニモードオフ
                                    case 0x7c:
                                        break;
                                    // オムニモードオン
                                    case 0x7d:
                                        break;
                                    // モノモードオン
                                    case 0x7e: 
                                        break;
                                     // モノモードオン
                                    case 0x7f: 
                                        break;
                                }
                            }
                            break;

                        case 0xc0: 
                            i += 1;
                            break;

                        case 0xd0: 
                            i += 1;
                            break;

                        case 0xe0: 
                            i += 2;
                            break;
                    }
                }

                else if(statusByte == 0x70 || statusByte == 0x7f)
                {
                    byte dataLength = data[i++];
                    i += dataLength;
                }

                else if(statusByte == 0xff)
                {
　　　　　　　　　　　// メタイベント
                    byte metaEventID = data[i++];
                    byte dataLength = data[i++];

                    switch (metaEventID)
                    {
                        // シーケンスメッセージ
                        case 0x00:
                            i += dataLength;
                            break;
                        // テキストイベント
                        case 0x01:  
                            i += dataLength;
                            break;
                        // 著作権表示
                        case 0x02:  
                            i += dataLength;
                            break;
                        // シーケンス/トラック名
                        case 0x03:  
                            i += dataLength;
                            break;
                        // 楽器名
                        case 0x04:  
                            i += dataLength;
                            break;
                        // 歌詞
                        case 0x05: 
                            i += dataLength;
                            break;
                        // マーカー
                        case 0x06:  
                            i += dataLength;
                            break;
                        // キューポイント
                        case 0x07:  
                            i += dataLength;
                            break;
                        // MIDIチャンネルプリフィクス
                        case 0x20:  
                            i += dataLength;
                            break;
                        // MIDIポートプリフィックス
                        case 0x21:  
                            i += dataLength;
                            break;
                        // トラック終了
                        case 0x2f:  
                            i += dataLength;
                            Fintrack=true;
                            // ここでループを抜ける
                            break;
                        // テンポ変更
                        case 0x51:  
                            {
                                // テンポ変更情報リストに格納する
                                var tempoData = new TempoData();
                                tempoData.eventTime = (int)CrTime;

                                // ４分音符の長さをマイクロ秒単位で格納されている
                                uint tempo = 0;
                                tempo |= data[i++];
                                tempo <<= 8;
                                tempo |= data[i++];
                                tempo <<= 8;
                                tempo |= data[i++];

                                // BPM割り出し
                                tempoData.bpm = 60000000 / (float)tempo;

                                // 小数点第1で切り捨て処理(10にすると第一位、100にすると第2位まで切り捨てられる)
                                tempoData.bpm = Mathf.Floor(tempoData.bpm * 10) / 10;

                                // tick値割り出し
                                tempoData.tick = (60 / tempoData.bpm / headerCH.division * 1000);

                                // リストにつっこむ
                                tempoList.Add(tempoData);
                            }
                            break;
                        // SMTPEオフセット
                        case 0x54: 
                            i += dataLength;
                            break;
                        // 拍子
                        case 0x58:
                        　　// 小節線を表示させるなら使えるかも
                            i += dataLength;
                            break;
                        // 調号
                        case 0x59:  
                            i += dataLength;
                            break;
                        // シーケンサ固有メタイベント
                        case 0x7f:  
                            i += dataLength;
                            break;
                    }
                }

                if(Fintrack) break;
        }
        // Debug.Log("FinAnalyz");
    }

    void ModificationEventTimes()
    {
        // 一時格納用（計算前の時間を保持したいため）
        var tempTempoList = new List<TempoData>(tempoList);

        // テンポイベント時間修正
        for (int i = 1; i < tempoList.Count; i++)
        {
            TempoData tempo = tempoList[i];

            int timeDifference = tempTempoList[i].eventTime - tempTempoList[i - 1].eventTime;
            tempo.eventTime = (int)(timeDifference * tempoList[i - 1].tick) + tempoList[i - 1].eventTime;

            tempoList[i] = tempo;
        }

        // ノーツイベント時間修正
        for (int i = 0; i < noteList.Count; i++)
        {
            for (int j = tempoList.Count - 1; j >= 0; j--)
            {
                if (noteList[i].eventTime >= tempTempoList[j].eventTime)
                {
                    NoteData note = noteList[i];

                    int timeDifference = noteList[i].eventTime - tempTempoList[j].eventTime;
                  　// 計算後のテンポ変更イベント時間+その時間
                    note.eventTime = (int)(timeDifference * tempTempoList[j].tick) + tempoList[j].eventTime;   
                    noteList[i] = note;
                    break;
                }
            }
        }
        // Debug.Log("FinEventTIme");
    }
    
    public bool [,] KeyCode;
    // public bool Ready=false;

    void Start(){
        // var fileName = @"C:\\Users\\famil\\OneDrive\\ドキュメント\\GitHub\\music_game\\test 3D\\Assets\\Scenes\\doremi.mid";
        // var fileName = @"C:\\Users\\famil\\OneDrive\\ドキュメント\\GitHub\\music_game\\test 3D\\doremi.mid";
        var fileName=@"C:\\Users\\famil\\OneDrive\\ドキュメント\\MIDI\\Alice in 冷凍庫 - コピー.mid";
        // var fileName=@"C:\Users\famil\Downloads\toruko.mid";

        LoadMSF(fileName);
        ModificationEventTimes();
        // Debug.Log(noteList.Count);
        // Debug.Log(tempoList.Count);
        // Debug.Log("EventTime");
        KeyCode=new bool[130,Enumerable.Last(noteList).eventTime*2];
        for(int i=0;i<noteList.Count;i++){
            // Debug.Log(noteList[i].laneIndex+" , "+noteList[i].eventTime/100);
            if(noteList[i].type != NoteType.LongEnd) KeyCode[noteList[i].laneIndex , noteList[i].eventTime/100] = true;
        }
        Ready=true;
    }
    
    private int i=0;
    private float CrTime;
    void Update()
    {
        if(Ready){
            CrTime += Time.deltaTime;
            if(CrTime > 0.1){
                for(int j=40;j<101;j++){
                    if(KeyCode[j,i]){
                        GameObject Notu=(GameObject)Resources.Load("Key");
                        Instantiate (Notu, new Vector3((((j+1)-70)*5),150f,-1f), Quaternion.identity);
                    }
                }
                CrTime = 0f;
                i++;
            }
        }
    }
}
