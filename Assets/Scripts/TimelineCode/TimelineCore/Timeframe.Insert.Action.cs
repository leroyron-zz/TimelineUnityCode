public partial class Timeline
{
    public partial class Core
    {
        public partial class Timeframe
        {
            public Action action = new Action();

            public class Action : Insert
            {
                public Action() : base()
                {
                    // The base constructor is called first.
                    // ... Then this code is executed.
                }
            }
        }
    }
}