using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Alauda.Enumerator
{
    public class SingleChildEnumerator : IEnumerator
    {
        internal SingleChildEnumerator(object Child)
        {
            _child = Child;
            _count = Child == null ? 0 : 1;
        }

        object IEnumerator.Current
        {
            get { return (_index == 0) ? _child : null; }
        }

        bool IEnumerator.MoveNext()
        {
            _index++;
            return _index < _count;
        }

        void IEnumerator.Reset()
        {
            _index = -1;
        }

        private int _index = -1;
        private int _count = 0;
        private object _child;
    }
}
