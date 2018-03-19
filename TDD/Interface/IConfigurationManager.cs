using System;
using System.Collections.Generic;
using System.Text;

namespace TDD.Interface
{
    public interface IConfigurationManager
    {
        string GetValue(string key);
    }
}
