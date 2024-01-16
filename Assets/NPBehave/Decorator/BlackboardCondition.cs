public class BlackboardCondition : ObservingDecorator
{

    Operator m_op;
    string m_key;
    object m_value;
    
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
        }
        
        return false;
    }
    protected override void StartObserving()
    {
       m_rootNode.Blackboard.AddObserver(m_key, Evaluate);
    }
    protected override void StopObserving()
    {
        m_rootNode.Blackboard.RemoveObserver(m_key);
    }
    
}
