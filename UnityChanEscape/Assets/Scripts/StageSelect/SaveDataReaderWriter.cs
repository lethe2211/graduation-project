using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// セーブデータを扱うクラス
public class SaveDataReaderWriter {

    public List<string> header { get; set; } // ヘッダ
    List<List<string>> data; // データ
    string filePath; // セーブデータがあるディレクトリのパス

    // コンストラクタ
    public SaveDataReaderWriter(string fileName)
    {
        header = new List<string>();
        data = new List<List<string>>();
        filePath = @"";

        LoadFile(fileName);
    }

    // "/Assets/CSVFile"内のファイル名を入力として受け取り，そのファイルを読み込む
    // Ex. sdrw.LoadFile(@"stageinfo.csv");
    public void LoadFile(string fileName)
    {
        try
        {
            using (StreamReader sr = new StreamReader(filePath + fileName, Encoding.GetEncoding("utf-8")))
            {
                // ヘッダの行の読み込み
                string firstLine = sr.ReadLine();
                string[] headerValue = firstLine.Split(',');
                header.AddRange(headerValue);

                // データの行の読み込み
                while (!sr.EndOfStream)
                {
                    List<string> rowData = new List<string>();
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');

                    for (int i = 0; i < values.Length; i++) rowData.Add(values[i]);
                    data.Add(rowData);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    // すべて読み込む
    public List<List<string>> ReadAll()
    {
        return data;
    }

    // 1行だけ読み込む
    public List<string> Read(int rowNum)
    {
        return data[rowNum];
    }

    // 1行単位で書き換える
    public void Update(int rowNum, List<string> rowData)
    {
        data[rowNum] = rowData;
    }

    // 末尾に付け足す
    public void Add(List<string> rowData)
    {
        data.Add(rowData);
    }

    // ファイル名を入力として受け取り，データを書き込む
    public void WriteFile(string fileName)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(filePath + fileName, false))
            {
                // ヘッダの行を書き込む
                string firstLine = "";

                for (int i = 0; i < header.Count; i++)
                {
                    if (i == 0) firstLine += header[i];
                    else firstLine += "," + header[i];
                }
                sw.WriteLine(firstLine);

                // データの行を書き込む
                for (int i = 0; i < data.Count; i++)
                {
                    string line = "";

                    for (int j = 0; j < data[i].Count; j++)
                    {
                        if (j == 0) line += data[i][j];
                        else line += "," + data[i][j];
                    }
                    sw.WriteLine(line);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    // 行数を返す
    public int CountRow()
    {
        return data.Count;
    }

    // 列数を返す
    public int CountCol()
    {
        return header.Count;
    }

}
