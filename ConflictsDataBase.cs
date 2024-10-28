using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Big_company_problem
{
    internal class ConflictsDataBase<T>
    {
        Dictionary<T, List<T>> _conflictMatrix;

        public ConflictsDataBase()
        {
            _conflictMatrix = new Dictionary<T, List<T>>();
        }
        
        public void AddConflict(T a, T b)
        {
            if(a is not null && b is not null)
            {
                if(!_conflictMatrix.ContainsKey(a))
                    _conflictMatrix.Add(a, new List<T>() { b });
                else
                {
                    if(!_conflictMatrix[a].Contains(b))
                        _conflictMatrix[a].Add(b);
                }
            }
        }

        public bool IsHaveConflict(T a, T b)
        {
            if(a is not null)
            {
                if(_conflictMatrix.ContainsKey(a))
                    return _conflictMatrix[a].Contains(b);
            }

            return false;
        }

        public bool IsHaveAnyConflict(T a, T b) => IsHaveConflict(a, b) || IsHaveConflict(b, a);
    }
}
