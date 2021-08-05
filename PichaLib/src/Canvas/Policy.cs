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

        public bool HasPixel(Pixel p)
        {
            if(this.Input == p.Name || this.Output == p.Name || this.ConditionB == p.Name)
            {
                return true;
            }
            return false;
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