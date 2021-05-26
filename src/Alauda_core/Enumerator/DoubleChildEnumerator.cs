using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alauda.Enumerator
{
    public class DoubleChildEnumerator : IEnumerator
    {
        internal DoubleChildEnumerator(object child1, object child2)
        {
            Debug.Assert(child1 != null, "First child should be non-null.");
            Debug.Assert(child2 != null, "Second child should be non-null.");

            _child1 = child1;
            _child2 = child2;
        }

        object IEnumerator.Current
        {
            get
            {
                switch (_index)
                {
                    case 0:
                        return _child1;
                    case 1:
                        return _child2;
                    default:
                        return null;
                }
            }
        }

        bool IEnumerator.MoveNext()
        {
            _index++;
            return _index < 2;
        }

        void IEnumerator.Reset()
        {
            _index = -1;
        }

        private int _index = -1;
        private object _child1;
        private object _child2;
    }
}
