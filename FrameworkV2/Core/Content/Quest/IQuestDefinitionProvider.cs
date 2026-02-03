using System.Collections.Generic;

// 퀘스트 정의 데이터를 외부에서 주입하기 위한 프로바이더 인터페이스
public interface IQuestDefinitionProvider
{
    // 특정 퀘스트 정의를 조회한다. 존재하지 않으면 false 반환
    bool TryGetDefinition(string questId, out QuestDefinition definition);
    // 전체 퀘스트 정의 목록을 반환한다
    IReadOnlyList<QuestDefinition> GetAll();
}
