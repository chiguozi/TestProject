using System.Collections;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;
using System.IO;
using UnityEditor;

public class MessagePackTest : MonoBehaviour
{
    Container c;

    void Start()
    {
        c = new Container();
        for(int i = 0; i < 10; i++)
        {
            c.map.Add(i, new ClassA());
        }
        for (int i = 0; i < 10; i++)
        {
            c.map.Add(10 + i, new ClassB());
        }
        MessagePackSerializer.SetDefaultResolver(MessagePack.Resolvers.StandardResolver.Instance);
        //MessagePack.Resolvers.CompositeResolver.RegisterAndSetAsDefault(MessagePack.Unity.UnityResolver.Instance);


    }
    void OnGUI()
    {
        if(GUILayout.Button("serialize"))
        {
            var bytes1 = MessagePackSerializer.Serialize(c);
            SaveFile("LZ4Bin", bytes1);
        }
        if (GUILayout.Button("Load"))
        {
            var bytes = Load("LZ4Bin");
            c = MessagePackSerializer.Deserialize<Container>(bytes);
            Debug.LogError("laod");
        }
        if (GUILayout.Button("serialize Json"))
        {
            var content = MessagePackSerializer.ToJson(c);
            SaveFile("LZ4Json", content);
        }

        if (GUILayout.Button("Load Json"))
        {
            var content = LoadText("LZ4Json");
        }
    }


    void SaveFile(string fileName, byte[] content)
    {
        File.WriteAllBytes(Application.dataPath + "/MessagePackTest/" + fileName + ".txt", content);
        AssetDatabase.Refresh();
    }

    void SaveFile(string fileName, string content)
    {
        File.WriteAllText(Application.dataPath + "/MessagePackTest/" + fileName + ".txt", content);
        AssetDatabase.Refresh();
    }

    byte[] Load(string fileName)
    {
        var bytes = File.ReadAllBytes(Application.dataPath + "/MessagePackTest/" + fileName + ".txt");
        return bytes;
    }

    string LoadText(string fileName)
    {
        var content = File.ReadAllText(Application.dataPath + "/MessagePackTest/" + fileName + ".txt");
        return content;
    }

}

[MessagePackObject(keyAsPropertyName:true)]
public class Container
{
    public Dictionary<int, Base> map = new Dictionary<int, Base>();
    public string name;
}
[MessagePackObject(keyAsPropertyName: true)]
public class Base
{
    public int IntField = 1;
    public float FloatField = 2f;
    public string StringField = "12";
    public Vector3 Vector = Vector3.one;
}
[MessagePackObject(keyAsPropertyName: true)]
public class ClassA : Base
{
    public int AField = 2;
}
[MessagePackObject(keyAsPropertyName: true)]
public class ClassB : Base
{
    public int BField = 3;
}

