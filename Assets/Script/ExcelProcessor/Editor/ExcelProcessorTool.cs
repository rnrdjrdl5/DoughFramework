
using System;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using MyEditorTools;

public class ExcelProcessorTool
{
    private const string SettingsPath = "Assets/Scripts/Common/ExcelProcessor/ExcelProcessorSettings.asset";

    [MenuItem("Dough/Tools/Excel Processor/Run")]
    public static void RunExcelProcessor()
    {
        ExcelProcessorSettings settings = GetOrCreateSettings();
        if (settings == null)
        {
            UnityEngine.Debug.LogError("Excel Processor Settings not found or could not be created.");
            return;
        }

        // Get project root path (parent of Assets folder)
        string projectRootPath = Path.GetDirectoryName(Application.dataPath);

        // Construct absolute paths
        string absoluteExcelFilePath = Path.Combine(projectRootPath, settings.excelFilePath.Replace("../", ""));
        string absoluteExecutablePath = Path.Combine(projectRootPath, settings.executablePath.Replace("../", ""));
        string absoluteGeneratedCodePath = Path.Combine(Application.dataPath, settings.generatedCodePath.Replace("Assets/", ""));
        string absoluteUserCodePath = Path.Combine(Application.dataPath, settings.userCodePath.Replace("Assets/", ""));
        string absoluteGameDataZipFolderPath = Path.Combine(Application.dataPath, settings.gameDataZipFolderPath.Replace("Assets/", ""));
        string absoluteGameDataZipFilePath = Path.Combine(absoluteGameDataZipFolderPath, settings.gameDataZipFileName);

        // Delete GameData and Code
        DeleteDirectoryIfExists(absoluteGeneratedCodePath);
        DeleteDirectoryIfExists(absoluteGameDataZipFolderPath);

        // Ensure output directories exist
        if (!Directory.Exists(absoluteGeneratedCodePath))
        {
            Directory.CreateDirectory(absoluteGeneratedCodePath);
        }
        if (!Directory.Exists(absoluteUserCodePath))
        {
            Directory.CreateDirectory(absoluteUserCodePath);
        }
        if (!Directory.Exists(absoluteGameDataZipFolderPath))
        {
            Directory.CreateDirectory(absoluteGameDataZipFolderPath);
        }

        // Validate paths
        if (!File.Exists(absoluteExcelFilePath))
        {
            UnityEngine.Debug.LogError($"Excel file not found: {absoluteExcelFilePath}");
            return;
        }
        if (!File.Exists(absoluteExecutablePath))
        {
            UnityEngine.Debug.LogError($"Executable not found at: {absoluteExecutablePath}");
            return;
        }

        // Construct arguments for the external process
        // Arguments: ExcelFilePath, GeneratedCodePath, UserCodePath, GameDataZipFileName
        string arguments = 
            $"\"{absoluteExcelFilePath}\" " +
            $"\"{absoluteGeneratedCodePath}\" " +
            $"\"{absoluteUserCodePath}\" " +
            $"\"{absoluteGameDataZipFilePath}\"";

        UnityEngine.Debug.Log($"Running Excel Processor with arguments: {arguments}");
        UnityEngine.Debug.Log($"Executable path: {absoluteExecutablePath}");

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = absoluteExecutablePath, // Use the executable path directly
            Arguments = arguments, // Pass arguments directly to the executable
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = new Process { StartInfo = startInfo })
        {
            try
            {
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    UnityEngine.Debug.Log($"Excel Processor completed successfully.\nOutput:\n{output}");
                }
                else
                {
                    UnityEngine.Debug.LogError($"Excel Processor failed with exit code {process.ExitCode}.\nOutput:\n{output}\nError:\n{error}");
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError($"Error running Excel Processor: {e.Message}");
            }
        }

        AssetDatabase.Refresh(); // Refresh Unity Asset Database to show newly generated files
    }

    [MenuItem("Dough/Tools/Excel Processor/Edit Settings")]
    public static void EditSettings()
    {
        Selection.activeObject = GetOrCreateSettings();
    }

    private static ExcelProcessorSettings GetOrCreateSettings()
    {
        ExcelProcessorSettings settings = AssetDatabase.LoadAssetAtPath<ExcelProcessorSettings>(SettingsPath);

        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<ExcelProcessorSettings>();
            AssetDatabase.CreateAsset(settings, SettingsPath);
            AssetDatabase.SaveAssets();
            UnityEngine.Debug.Log($"Created new Excel Processor Settings asset at: {SettingsPath}");
        }
        return settings;
    }

    static void DeleteDirectoryIfExists(string dirPath)
    {
        try
        {
            Directory.Delete(dirPath, recursive: true);
        }
        catch (DirectoryNotFoundException)
        {
            
        }
    }
}
