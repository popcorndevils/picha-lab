namespace PichaLib
{
    public class Policy
    {
        public string Input;
        public string Output;
        public float Rate;
        public ConditionTarget ConditionA = ConditionTarget.NONE;
        public ConditionExpression ConditionLogic = ConditionExpression.NONE;
        public string ConditionB;

        public void VoidValue(Pixel p)
        {
            if(this.Input == p.Name) { this.Input = "[NULL]"; }
            if(this.Output == p.Name) { this.Output = "[NULL]"; }
        }
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