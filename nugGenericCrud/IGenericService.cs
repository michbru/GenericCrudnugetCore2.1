using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace nugGenericCrud
{
    public interface IGenericService
    {
        Task<IEnumerable<dynamic>> GetAllItems(string entName);
        Task<IEnumerable<dynamic>> GetItemsSQL(APISend values);
        dynamic UpdateItem(dynamic item);
        dynamic DeleteItem(dynamic item);
    }
}
