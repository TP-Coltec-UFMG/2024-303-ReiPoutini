using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SistemadeSave {
    public static void SalvarDados(DadosDoJogo dados) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, dados);
        stream.Close();
    }

    public static DadosDoJogo pegardados() {
        string path = Application.persistentDataPath + "/save.dat";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DadosDoJogo dados = formatter.Deserialize(stream) as DadosDoJogo;
            stream.Close();
            return dados;
        } else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
