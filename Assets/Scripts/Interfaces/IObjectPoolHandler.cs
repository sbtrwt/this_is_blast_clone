using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaster.Interfaces
{
    public interface IObjectPoolHandler<T> where T : class
    {
        void ReturnItem(T item);
    }
}
