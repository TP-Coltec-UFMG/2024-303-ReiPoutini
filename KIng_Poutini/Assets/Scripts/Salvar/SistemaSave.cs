using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SistemadeSave {

    public static void SalvarDados(DadosDoJogo dados) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/saveData.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, dados);
        stream.Close();
    }

    public static DadosDoJogo pegardados() {
        string path = Application.persistentDataPath + "/saveData.data";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DadosDoJogo dados = formatter.Deserialize(stream) as DadosDoJogo;
            stream.Close();
            return dados;
        } else {
            return null;
        }
    }
}