using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PostProcessBuild : IPostprocessBuildWithReport
{
  public void OnPostprocessBuild(BuildReport report)
  {
    var outputPath = report.summary.outputPath;
    var outputFolder = Path.GetDirectoryName(outputPath);
    if (outputFolder == null)
      throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {outputPath}");

    CopyDirectory("data", $"{outputFolder}/data", true);
  }

  public int callbackOrder => 0;

  private static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
  {
    var dir = new DirectoryInfo(sourceDirName);

    if (!dir.Exists)
      throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDirName}");

    var dirs = dir.GetDirectories();
    if (Directory.Exists(destDirName) == false)
      Directory.CreateDirectory(destDirName);

    var files = dir.GetFiles();
    foreach (var file in files)
    {
      string tempPath = Path.Combine(destDirName, file.Name);
      file.CopyTo(tempPath, false);
    }

    if (copySubDirs)
    {
      foreach (var subDir in dirs)
      {
        string tempPath = Path.Combine(destDirName, subDir.Name);
        CopyDirectory(subDir.FullName, tempPath, true);
      }
    }
  }
}
