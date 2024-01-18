using UnityEngine;
public class BlackboardCondition : ObservingDecorator
{
    Operator m_op;
    public Operator Operator => m_op;
    string m_key;
    public string Key => m_key;
    
    object m_value;
    public object Value => m_value;
    
    public BlackboardCondition(string _key, Operator _op, object _result, STOPS _stop, Node _decorator) : base("BlackboardCondition", _decorator)
    {
        m_stop = _stop;
        m_op = _op;
        m_key = _key;
        m_value = _result;
    }
    
    protected override bool IsConditionMet()
    {
        var keyValue = m_rootNode.Blackboard.Get(m_key);
        
        switch (m_op)
        {
            case Operator.IS_EQUAL: 
                return Equals(m_value, keyValue);
            case Operator.IS_SMALLER:
            {
                if (keyValue is float && m_value is float)
                {
                    var orig = (float)keyValue;
                    var target = (float)m_value;

                    return orig < target;
                }
                
                if (keyValue is int && m_value is int)
                {
                    var orig = (float)keyValue;
                    var target = (float)m_value;

                    return orig < target;
                }
                
                Debug.LogError("Type not compareable: " + keyValue.GetType());

                return false;
            }
            default:
                return false;
        }
    }
    
    protected override void StartObserving()
    {
       m_rootNode.Blackboard.AddObserver(m_key, Evaluate);
    }
    
    protected override void StopObserving()
    {
        m_rootNode.Blackboard.RemoveObserver(m_key, Evaluate);
    }
}
