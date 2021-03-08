namespace PichaLib
{
    public class Policy
    {
        public int Input;
        public int Output;
        public float Rate;
        public ConditionTarget ConditionA = ConditionTarget.NONE;
        public ConditionExpression ConditionLogic = ConditionExpression.NONE;
        public int ConditionB;
    }

    public enum ConditionTarget
    {
        NONE,
        NEIGHBOR
    }

    public enum ConditionExpression
    {
        NONE,
        IS,
        IS_NOT
    }
}