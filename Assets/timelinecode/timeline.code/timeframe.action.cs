partial class TIMELINE
{
    public partial class CODE
    {
        public partial class TIMEFRAME
        {
            public ACTION action = new ACTION();

            public class ACTION : INSERT
            {
                public ACTION() : base()
                {
                    // The base constructor is called first.
                    // ... Then this code is executed.
                }
            }
        }
    }
}