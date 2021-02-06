using UnityEditor;

public class BuildScripts
{
    [MenuItem("Build/WebGL")]
    public static void BuildWebGL()
    {
        var options = new BuildPlayerOptions
        {
            locationPathName = "Build/",
            scenes = new string[]
            {
                "Assets/Scenes/Menu.unity",
                "Assets/Scenes/GameScene.unity"
            },
            targetGroup = BuildTargetGroup.WebGL,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };
        BuildPipeline.BuildPlayer(options);
    }
}
