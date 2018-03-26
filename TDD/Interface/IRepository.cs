using System;
using System.Collections.Generic;
using System.Text;

namespace TDD.Interface
{
    public interface IRepository<T> where T : Entity
    {
        bool Exist(Func<User, bool> function);
        T GetBy(Func<User, bool> function);
        void Insert(User user);
    }
}
