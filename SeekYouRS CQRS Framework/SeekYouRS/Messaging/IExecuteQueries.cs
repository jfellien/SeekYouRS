using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeekYouRS.Messaging
{
    public interface IExecuteQueries
    {
        T Retrieve<T>(dynamic query);
    }
}
