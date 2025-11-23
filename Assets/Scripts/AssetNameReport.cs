using UnityEngine;
using System.Linq;
using System.IO;
using System.Text;

public class AssetNameReport : MonoBehaviour
{
    void Start()
    {
        // 1. Load all sprites and audio clips from Resources
        var images = Resources.LoadAll<Sprite>("Images")
                              .Select(s => s.name).ToList();
        var audio = Resources.LoadAll<AudioClip>("Audio")
                              .Select(a => a.name).ToList();

        // 2. Build a big string with all the info
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("===== IMAGE FILES =====");
        foreach (var img in images)
            sb.AppendLine(img);

        sb.AppendLine();
        sb.AppendLine("===== AUDIO FILES =====");
        foreach (var snd in audio)
            sb.AppendLine(snd);

        sb.AppendLine();
        sb.AppendLine("===== IMAGES WITH NO AUDIO =====");
        foreach (var img in images)
            if (!audio.Contains(img))
                sb.AppendLine("NO AUDIO: " + img);

        sb.AppendLine();
        sb.AppendLine("===== AUDIO WITH NO IMAGE =====");
        foreach (var snd in audio)
            if (!images.Contains(snd))
                sb.AppendLine("NO IMAGE: " + snd);

        // 3. Write to a text file in your Assets folder
        string path = Path.Combine(Application.dataPath, "AssetReport.txt");
        File.WriteAllText(path, sb.ToString());

        Debug.Log("✅ Asset report written to: " + path);

        // 4. Open the text file automatically (Notepad on Windows)
        // This uses the OS default program for .txt
        Application.OpenURL("file:///" + path.Replace("\\", "/"));
    }
}
