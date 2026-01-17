using System;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class FormulaParser : MonoBehaviour
{
    static string OperateStr = "+-*/()";
    
    string[] tokens;
    int pos;

    public IFormulaNode Parse(string expr)
    {
        tokens = Tokenize(expr);
        pos = 0;
        
        return ParseExpression();
    }

    string[] Tokenize(string splitData)
    {
        var list = new List<string>();
        var token = "";

        foreach (var ch in splitData)
        {
            if (char.IsWhiteSpace(ch))
            {
                continue;
            }

            if (OperateStr.Contains(ch))
            {
                if (token != "")
                {
                    list.Add(token);
                    token = "";
                }
                
                list.Add(ch.ToString());
            }
            else
            {
                token += ch;
            }
        }

        if (token != "")
        {
            list.Add(token);
        }
        
        return list.ToArray();
    }

    IFormulaNode ParseExpression()
    {
        var node = ParseTerm();

        while (pos < tokens.Length && (tokens[pos] == "+" || tokens[pos] == "-"))
        {
            var op = tokens[pos++];
            var right = ParseTerm();
            
            node = new OperateNode(op, node, right);
        }

        return node;
    }

    IFormulaNode ParseTerm()
    {
        var node = ParseFactor();

        while (pos < tokens.Length && (tokens[pos] == "*" || tokens[pos] == "/"))
        {
            var op = tokens[pos++];
            var right = ParseFactor();
            
            node = new OperateNode(op, node, right);
        }
        
        return node;
    }

    IFormulaNode ParseFactor()
    {
        string token = tokens[pos++];

        if (float.TryParse(token, out float num))
        {
            return new NumberNode(num);
        }

        if (token == "(")
        {
            var node = ParseExpression();
            
            pos++;
            
            return node;
        }

        return new VariableNode(token);
    }
}

public static class FormulaParserExtensions
{
    static string FormulaStartsWith = ": ";
    
    public static bool TryGetFormulaValue(this string targetString, out float result, IReadOnlyDictionary<string, Func<float>> func = null)
    {
        if (targetString.StartsWith(FormulaStartsWith))
        {
            targetString = targetString.Substring(FormulaStartsWith.Length);
            
            var formula = new FormulaParser();
            var node = formula.Parse(targetString);
        
            result = node.Calculate(func);
            
            return true;
        }

        else
        {
            result = 0.0f;
            
            return false;
        }
    }
}