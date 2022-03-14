using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Shared.Helpers
{
    //>>>This is a class containing generic color and sprite functions

    public class IO {

        public static void Share (string VIDEO_NAME = "Record") {
            string path = Path.Combine(Application.persistentDataPath, VIDEO_NAME + ".mp4");
            //new NativeShare().AddFile(path, "video/mp4").SetSubject("www.heavycourt.com").SetText(Project.appUrl).Share();
        }

        public static string GetPath(string filename) { // Returns the used path with filename and its ending
            return Path.Combine(Application.persistentDataPath, filename);
        }

        public static bool FileExists(string name) {
            return File.Exists(Path.Combine(Application.persistentDataPath, name));
        }

        public static void SaveFile(byte[] bytes, string name, bool overwrite = true) { // save file from persistent path
            string destination = Path.Combine(Application.persistentDataPath, name);
            if (overwrite || !File.Exists(destination))
                File.WriteAllBytes(destination, bytes);
        }

        public static Sprite LoadImage (string name) {// load image from persistent path
		    string destination = Path.Combine(Application.persistentDataPath, name + ".png");
		    if (File.Exists(destination))
			    return TextureToSprite(LoadTexture (destination)); 
		    return null;  //if not exist then create first
	    }

        public static void SaveImage (Texture2D texture, string name, bool overwrite = true, bool freeMemory = true) {// save image from persistent path
		    string destination = Path.Combine(Application.persistentDataPath, name + ".png");
		    if (overwrite || !File.Exists (destination))
			    File.WriteAllBytes(destination, texture.EncodeToPNG());
            if (freeMemory)
                GameObject.Destroy(texture); // for free the memory again
        }

        public static void DeleteImage(string name)
        {
		    string path = Path.Combine(Application.persistentDataPath, name + ".png");
            if (File.Exists(path))
                File.Delete(path); //Delete photo permenantly
        }

        public static Texture2D LoadTexture(string destination) {
            Texture2D texture = new Texture2D(0, 0, TextureFormat.RGBA32, true);
            texture.LoadImage(File.ReadAllBytes(destination));
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            return texture;
        }

        public static Sprite TextureToSprite(Texture2D texture) {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
        }

        // Assigns the loaded audio clip to the source or does nothing if the argument filename or path is inexistend. Call this method inside the StartCoroutine of C#
        public static IEnumerator LoadAudio(string path, Action<AudioClip> action) {
            if (File.Exists(path)) {
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + path, AudioType.MPEG)) {
                    yield return www.SendWebRequest();
                    if (www.isNetworkError)
                        Debug.Log(www.error);
                    else
                        action(DownloadHandlerAudioClip.GetContent(www));
                }
            }
        }
    }
}
