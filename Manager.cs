using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Manager : MonoBehaviour
{
    public RenderTexture rt;
    public Camera captureCam;

    void Update()
    {
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            TakePicture();
        }
    }

    private void TakePicture()
    {
        var tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        captureCam.Render();
        RenderTexture.active = rt;

        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        Destroy(tex);

        string currentTime = DateTime.Now.ToString("yyy-mm-dd_HH-mm-ss");

        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
        string photoPath = Path.Combine(directoryInfo.Parent.FullName, "Photo");

        if (!Directory.Exists(photoPath))
        {
            Directory.CreateDirectory(photoPath);
        }
        File.WriteAllBytes($"{photoPath}/SnapShot_{currentTime}.jpg", tex.EncodeToJPG());
    }
    private void OnApplicationQuit()
    {
        rt.Release();
    }
}
