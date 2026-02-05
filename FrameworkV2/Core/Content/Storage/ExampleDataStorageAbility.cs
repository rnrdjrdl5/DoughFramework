using System.Collections.Generic;

// DataStorageAbility 사용 예시를 보여주는 참고 스크립트
public static class ExampleDataStorageAbility
{
    // Example: 외부 이벤트로 저장/로드 요청을 처리한다
    public static void RunExample()
    {
        var storageAbility = new DataStorageAbility();

        storageAbility.Register(new DemoSaveData(coin: 100, itemCount: 3));
        storageAbility.Register(new DemoQuestData(progress: 2));

        storageAbility.SaveRequested += payloads =>
        {
            var _ = payloads;
        };

        storageAbility.LoadRequested += payloads =>
        {
            if (payloads == null)
            {
                return;
            }

            payloads[typeof(DemoSaveData).FullName] = new DemoSaveData(coin: 200, itemCount: 5);
            payloads[typeof(DemoQuestData).FullName] = new DemoQuestData(progress: 4);
        };

        storageAbility.TryRequestSave();
        storageAbility.RequestLoad();
    }

    // 예시용 저장 데이터
    sealed class DemoSaveData : SavePayload
    {
        public int Coin { get; }
        public int ItemCount { get; }

        public DemoSaveData(int coin, int itemCount)
            : base()
        {
            Coin = coin;
            ItemCount = itemCount;
        }
    }

    // 예시용 퀘스트 데이터
    sealed class DemoQuestData : SavePayload
    {
        public int Progress { get; }

        public DemoQuestData(int progress)
            : base()
        {
            Progress = progress;
        }
    }
}
