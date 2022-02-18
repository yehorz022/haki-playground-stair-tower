using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconMakerForMesh : MonoBehaviour
{
    public GameObject prefab;
    public SpriteRenderer sp;
    public Camera componentCamera;

    void Start()
    {
        //componentCamera = new Camera();
        //componentCamera = gameObject.AddComponent<Camera>();
        sp.sprite = Graphik.TextureToSprite (CreateTexture(Instantiate(prefab)));
    }

    private Texture2D CreateTexture(GameObject go)
    {
        const int size = 600;
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        RenderTexture renderTexture = new RenderTexture(size, size, 24);
        Texture2D screenShot = new Texture2D(size, size, TextureFormat.RGBA32, false);

        componentCamera.targetTexture = renderTexture;
        componentCamera.Render();
        RenderTexture.active = renderTexture;

        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        return screenShot;
    }
}
