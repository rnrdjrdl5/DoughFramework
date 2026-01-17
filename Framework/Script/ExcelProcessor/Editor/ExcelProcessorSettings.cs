
using UnityEngine;

namespace MyEditorTools // New namespace
{
    [CreateAssetMenu(fileName = "ExcelProcessorSettings", menuName = "Tools/Excel Processor Settings", order = 1)]
    public class ExcelProcessorSettings : ScriptableObject
    {
        [Tooltip("엑셀 파일 경로 (예: ../Data/GameData.xlsx). Unity 프로젝트의 Assets 폴더의 부모 폴더에 위치합니다.")]
        public string excelFilePath;

        [Tooltip("생성된 코드가 저장될 Unity 프로젝트 내의 경로 (예: Assets/Scripts/Generated). 'Assets/'로 시작해야 합니다.")]
        public string generatedCodePath;

        [Tooltip("사용자 정의 코드가 저장될 Unity 프로젝트 내의 경로 (예: Assets/Scripts/UserCode). 'Assets/'로 시작해야 합니다.")]
        public string userCodePath;

        [Tooltip("생성될 게임 데이터 ZIP 파일이 저장될 Unity 프로젝트 내의 폴더 경로 (예: Assets/GameData/Output). 'Assets/'로 시작해야 합니다.")]
        public string gameDataZipFolderPath;

        [Tooltip("생성될 게임 데이터 ZIP 파일의 이름 (예: GameData.zip).")]
        public string gameDataZipFileName;

        [Tooltip("외부 실행 파일 경로 (예: ../ExcelProcessor.dll). Unity 프로젝트의 Assets 폴더의 부모 폴더에 위치합니다. .NET 런타임에 의해 실행될 .dll 파일이어야 합니다.")]
        public string executablePath;
    }
}
