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
  Normal,      // 通常ノーツ
  LongStart,   // ロング開始
  LongEnd,     // ロング終端
}

public struct NoteData
{
  public int eventTime;  // ノーツタイミング(ms)
  public int laneIndex;  // レーン番号
  public NoteType type;   // ノーツの種類
}

public struct TempoData
{
  public int eventTime;  // BPM変化のタイミング(ms)
  public float bpm;      // BPM値
  public float tick;      // tick値
}



/// ヘッダーチャンク情報を格納する構造体
public struct HeaderChunkData
{
    public byte[] chunkID;      // チャンクのIDを示す(4byte)
    public int dataLength;      // チャンクのデータ長(4byte)
    public short format;        // MIDIファイルフォーマット(2byte)
    public short tracks;        // トラック数(2byte)
    public short division;      // タイムベース MIDI独自の時間の最小単位をtickと呼び、4分音符あたりのtick数がタイムベース 大体480(2byte)
};

/// トラックチャンク情報を格納する構造体
public struct TrackChunkData
{
    public byte[] chunkID;      // チャンクのIDを示す(4byte)
    public int dataLength;      // チャンクのデータ長(4byte)
    public byte[] data;         // 演奏情報が入っているデータ
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
                // ヘッダ部のデータ長(値は6固定)
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
                // ヘッダ部のデータ長(値は6固定)
                headerCH.dataLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                // フォーマット(2byte)
                headerCH.format = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                // トラック数(2byte)
                headerCH.tracks = BitConverter.ToInt16(reader.ReadBytes(2), 0);
                // タイムベース(2byte)
                headerCH.division = BitConverter.ToInt16(reader.ReadBytes(2), 0);
            }
            
            // トラックチャンク侵入
            var trackCH = new TrackChunkData[headerCH.tracks];
            
            // トラック数ぶん
            for(int i=0;i<headerCH.tracks;i++){

                // チャンクID
                trackCH[i].chunkID = reader.ReadBytes(4);

                // 自分のPCがリトルエンディアンなら変換する
                if (BitConverter.IsLittleEndian)
                {
                    // トラックのデータ長読み込み(値は6固定)
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

                // トラックデータ解析に回す
                TrackDataAnalys(trackCH[i].data,headerCH);
            }
        }
    }

    void TrackDataAnalys(byte[] data,HeaderChunkData headerCH){
        // Debug.Log(data.Length);
        uint CrTime=0;                     // 現在の時間 [ ms ]
        byte statusByte=0;                 // ステータスバイト
        bool[] longFlags=new bool[128];    // ロングノーツ用フラグ

        for(int i=0;i<data.Length;){

            // Debug.Log(i);
            
            bool Fintrack=false;
            uint deltaTime=0;

            while(true){
                var tmp = data[i++];

                // 下位7bitを格納
                deltaTime |= (tmp & (uint)0x7f);

                // 最上位1bitが0ならデータ終了
                if ((tmp & 0x80) == 0) break;

                // 次の下位7bit用にビット移動
                deltaTime = deltaTime << 7;
            }

            // 現在の時間にデルタタイムを足す
            CrTime+=deltaTime;
            
            /* ランニングステータスチェック */
            if (data[i] >= 0x80)
            {
                statusByte = data[i++]; // ステータスバイト保存
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
                        dataByte0 = data[i++];// どのキーが離されたか
                        // ベロシティ値
                        dataByte1 = data[i++];

                        // 前のレーンがロングノーツなら
                        if (longFlags[dataByte0])
                        {
                            // ロング終点ノート情報生成
                            var note = new NoteData();
                            note.eventTime = (int)CrTime;
                            note.laneIndex = (int)dataByte0;
                            note.type = NoteType.LongEnd;

                            // リストにつっこむ
                            noteList.Add(note);

                            // ロングノーツフラグ解除
                            longFlags[note.laneIndex] = false;
                        }
                        break;
                    case 0x90:  // ノートオン(ノートオフが呼ばれるまでは押しっぱなし扱い)
                            // どのキーが押されたか
                            dataByte0 = data[i++];
                            // ベロシティ値という名の音の強さ。ノートオフメッセージの代わりにここで0を送ってくるタイプもある
                            dataByte1 = data[i++];

                            {
                                // ノート情報生成
                                var note = new NoteData();
                                note.eventTime = (int)CrTime;
                                note.laneIndex = (int)dataByte0;
                                note.type = NoteType.Normal;
                                // 独自でやっている。ベロシティ値が最大のときのみロングの始点とする
                                if (dataByte1 == 127)
                                {
                                   note.type = NoteType.LongStart;
                                   // ロングノーツフラグセット
                                   longFlags[note.laneIndex] = true;
                                }
                                // ノートオフイベントではなく、ベロシティ値0をノートオフとして保存する形式もあるので対応
                                if (dataByte1 == 0)
                                {
                                    // 同じレーンで前回がロングノーツ始点なら
                                    if (longFlags[note.laneIndex])
                                    {
                                        note.type = NoteType.LongEnd;
                                        // ロングノーツフラグ解除
                                        longFlags[note.laneIndex] = false;
                                    }
                                }

                                // リストにつっこむ
                                noteList.Add(note);
                            }
                            break;
                        case 0xa0:  // ポリフォニック キープレッシャー(鍵盤楽器で、キーを押した状態でさらに押し込んだ際に、その圧力に応じて送信される)
                            i += 2; // 使わないのでスルー
                            break;
                        case 0xb0:  // コントロールチェンジ(音量、音質など様々な要素を制御するための命令)
                            // コントロールする番号
                            dataByte0 = data[i++];
                            // 設定する値
                            dataByte1 = data[i++];

                            // ※0x00-0x77までがコントロールチェンジで、それ以上はチャンネルモードメッセージとして処理する
                            if (dataByte0 < 0x78)
                            {
                                // コントロールチェンジ
                            }
                            else
                            {
                                // チャンネルモードメッセージは一律データバイトを2つ使用している
                                // チャンネルモードメッセージ
                                switch (dataByte0)
                                {
                                    case 0x78:  // オールサウンドオフ
                                        // 該当するチャンネルの発音中の音を直ちに消音する。後述のオールノートオフより強制力が強い。
                                        break;
                                    case 0x79:  // リセットオールコントローラ
                                        // 該当するチャンネルの全種類のコントロール値を初期化する。
                                        break;
                                    case 0x7a:  // ローカルコントロール
                                        // オフ:鍵盤を弾くとMIDIメッセージは送信されるがピアノ自体から音は出ない
                                        // オン:鍵盤を弾くと音源から音が出る(基本こっち)
                                        break;
                                    case 0x7b:  // オールノートオフ
                                        // 該当するチャンネルの発音中の音すべてに対してノートオフ命令を出す
                                        break;
                                    /* MIDIモード設定 */
                                    // オムニのオン・オフとモノ・ポリモードを組み合わせて4種類のモードがある
                                    case 0x7c:  // オムニモードオフ
                                        break;
                                    case 0x7d:  // オムニモードオン
                                        break;
                                    case 0x7e:  // モノモードオン
                                        break;
                                    case 0x7f:  // モノモードオン
                                        break;
                                }
                            }
                            break;

                        case 0xc0:  // プログラムチェンジ(音色を変える命令)
                            i += 1;
                            break;

                        case 0xd0:  // チャンネルプレッシャー(概ねポリフォニック キープレッシャーと同じだが、違いはそのチャンネルの全ノートナンバーに対して有効となる)
                            i += 1;
                            break;

                        case 0xe0:  // ピッチベンド(ウォェーンウェューンの表現で使う)
                            i += 2;
                            // ボルテのつまみみたいなのを実装する場合、ここの値が役立つかも
                            break;
                    }
                }

                /* システムエクスクルーシブ (SysEx) イベント*/
                else if(statusByte == 0x70 || statusByte == 0x7f)
                {
                    byte dataLength = data[i++];
                    i += dataLength;
                }

                /* メタイベント*/
                else if(statusByte == 0xff)
                {
                    // メタイベントの番号
                    byte metaEventID = data[i++];
                    // データ長
                    byte dataLength = data[i++];

                    switch (metaEventID)
                    {
                        case 0x00:  // シーケンスメッセージ
                            i += dataLength;
                            break;
                        case 0x01:  // テキストイベント
                            i += dataLength;
                            break;
                        case 0x02:  // 著作権表示
                            i += dataLength;
                            break;
                        case 0x03:  // シーケンス/トラック名
                            i += dataLength;
                            break;
                        case 0x04:  // 楽器名
                            i += dataLength;
                            break;
                        case 0x05:  // 歌詞
                            i += dataLength;
                            break;
                        case 0x06:  // マーカー
                            i += dataLength;
                            break;
                        case 0x07:  // キューポイント
                            i += dataLength;
                            break;
                        case 0x20:  // MIDIチャンネルプリフィクス
                            i += dataLength;
                            break;
                        case 0x21:  // MIDIポートプリフィックス
                            i += dataLength;
                            break;
                        case 0x2f:  // トラック終了
                            i += dataLength;
                            Fintrack=true;
                            // ここでループを抜けても良い
                            break;
                        case 0x51:  // テンポ変更
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
                        case 0x54:  // SMTPEオフセット
                            i += dataLength;
                            break;
                        case 0x58:  // 拍子
                            i += dataLength;
                            // 小節線を表示させるなら使えるかも
                            break;
                        case 0x59:  // 調号
                            i += dataLength;
                            break;
                        case 0x7f:  // シーケンサ固有メタイベント
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
                    note.eventTime = (int)(timeDifference * tempTempoList[j].tick) + tempoList[j].eventTime;   // 計算後のテンポ変更イベント時間+その時間
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