
// NOTE : 재활용 필요 시, 변수를 Data로 분리해서 관리하기.
// 단, 어떤 Node의 데이터인지 명시할 방법이 필요하다.
public class CooldownDecorator : DecoratorNode
{
    public override BTNodeState OnUpdateNode()
    {
        return base.OnUpdateNode();
    }
}