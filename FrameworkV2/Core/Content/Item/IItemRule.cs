// 아이템 수량 변경 규칙 인터페이스
public interface IItemRule
{
    // 수량 추가 가능 여부를 확인한다
    bool CanAdd(int id, int amount, out string reason);
    // 수량 차감 가능 여부를 확인한다
    bool CanRemove(int id, int amount, out string reason);
    // 수량 설정 가능 여부를 확인한다
    bool CanSetAmount(int id, int amount, out string reason);
}
