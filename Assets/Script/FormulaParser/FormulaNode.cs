using System.Collections.Generic; 
using System;

public interface IFormulaNode
{
    float Calculate(IReadOnlyDictionary<string, Func<float>> dict);
}

public class VariableNode : IFormulaNode
{
    public string Name => name;
    
    string name;

    public VariableNode(string name)
    {
        this.name = name;
    }

    public float Calculate(IReadOnlyDictionary<string, Func<float>> dict)
    {
        if (dict.TryGetValue(name, out var func))
        {
            return func();
        }
        
        throw new Exception($"Can't find variable {name}");
    }
}

public class NumberNode : IFormulaNode
{
    public float Value => value;
    
    float value;

    public NumberNode(float value)
    {
        this.value = value;
    }

    public float Calculate(IReadOnlyDictionary<string, Func<float>> dict) => value;
}

public class OperateNode : IFormulaNode
{
    public string Operate => operate;

    IFormulaNode LNode => lNode;
    IFormulaNode RNode => rNode;
    
    IFormulaNode lNode;
    IFormulaNode rNode;
    
    string operate;
    
    public OperateNode(string operate, IFormulaNode lNode, IFormulaNode rNode)
    {
        this.operate = operate;
        this.lNode = lNode;
        this.rNode = rNode;
    }

    public float Calculate(IReadOnlyDictionary<string, Func<float>> dict)
    {
        float leftValue = lNode.Calculate(dict);
        float rightValue = rNode.Calculate(dict);

        return operate switch
        {
            "+" => leftValue + rightValue,
            "-" => leftValue - rightValue,
            "*" => leftValue * rightValue,
            "/" => leftValue / rightValue,
            _ => throw new Exception($"Can't calculate variable {operate}")
        };
    }
}
