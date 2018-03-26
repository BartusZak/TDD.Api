using System;
using System.Collections.Generic;
using System.Text;

namespace TDD.Interface
{
    public interface IRepository<T> where T : Entity
    {
        bool Exist(Func<T, bool> function);
        T GetBy(Func<T, bool> function);
        void Insert(T user);
    }
}
