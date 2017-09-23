using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapShop.Design.MongoDb
{
    public interface IRepository<T>
    {
        void Add(T product, string collection);
        List<T> GetByKeywords(List<string> keywords);
    }
}
