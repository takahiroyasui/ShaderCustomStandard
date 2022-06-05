using System;
using System.IO;
using UnityEngine;

/// <summary>
/// 指定されたカメラの内容をキャプチャするサンプル
/// https://nekojara.city/unity-screenshot
/// </summary>
public class CameraScreenShotCapture : MonoBehaviour {
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CaptureScreenShot(DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".jpg");
        }
    }
    
    private void CaptureScreenShot(string filePath) {
        var mainCamera = Camera.main;
        var rt = new RenderTexture(mainCamera.pixelWidth, mainCamera.pixelHeight, 24);
        var prev = mainCamera.targetTexture;
        mainCamera.targetTexture = rt;
        mainCamera.Render();
        mainCamera.targetTexture = prev;
        RenderTexture.active = rt;

        var screenShot = new Texture2D(
            mainCamera.pixelWidth,
            mainCamera.pixelHeight,
            TextureFormat.RGB24,
            false);
        screenShot.ReadPixels(new Rect(0, 0, screenShot.width, screenShot.height), 0, 0);
        screenShot.Apply();

        var bytes = screenShot.EncodeToJPG();
        Destroy(screenShot);

        File.WriteAllBytes(filePath, bytes);
        Debug.Log($"スクリーンショットを保存しました： {filePath}");
    }
}