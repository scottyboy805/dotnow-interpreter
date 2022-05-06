#if API_NET35

namespace System
{
    public sealed class Lazy<T>
    {
        // Private
        private readonly object padlock = new object();
        private readonly Func<T> createValue;
        private bool isValueCreated;
        private T value;

        // Properties
        public T Value
        {
            get
            {
                if (!isValueCreated)
                {
                    lock (padlock)
                    {
                        if (!isValueCreated)
                        {
                            value = createValue();
                            isValueCreated = true;
                        }
                    }
                }
                return value;
            }
        }

        public bool IsValueCreated
        {
            get
            {
                lock (padlock)
                {
                    return isValueCreated;
                }
            }
        }

        // Constructor
        public Lazy(Func<T> createValue)
        {
            if (createValue == null) throw new ArgumentNullException("createValue");

            this.createValue = createValue;
        }

        // Methods
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
#endif
