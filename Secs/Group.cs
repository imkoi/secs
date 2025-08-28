using System.Collections;
using System.Collections.Generic;

namespace Secs
{
    public struct Group
    {
        public Enumerator GetEnumerator()
        {
            return new Enumerator();
        }

        public struct Enumerator : IEnumerator<int>
        {
            public void Dispose()
            {
                // TODO release managed resources here
            }

            public bool MoveNext()
            {
                throw new System.NotImplementedException();
            }

            public void Reset()
            {
                throw new System.NotImplementedException();
            }

            public int Current { get; }

            object IEnumerator.Current => Current;
        }
    }
}