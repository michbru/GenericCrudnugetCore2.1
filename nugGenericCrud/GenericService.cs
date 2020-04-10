using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace nugGenericCrud
{
    public class GenericService : IGenericService
    {
        private readonly DbContext _appDbContext;
        public GenericService(DbContext appDbContext) 
        {
            this._appDbContext = appDbContext;
        }


        public async Task<IEnumerable<dynamic>> GetAllItems(string entName)
        {
            var ctx = this._appDbContext;
            var ctxType = ctx.GetType();

            var test = ctxType.GetProperty(entName);
            dynamic ent = (ctxType.GetProperty(entName).GetValue(ctx, null));


            return ent;
        }


        public async Task<IEnumerable<dynamic>> GetItemsSQL(APISend values)
        {
            string _entity = values.p_entity;
            string _entity_sql = values.p_entity_sql;
            string _sqlWhere = values.p_sqlWhere;
            string _sqlOrder = values.p_sqlOrder;

            string sql;
            if (string.IsNullOrEmpty(_entity_sql) == true) { _entity_sql = _entity; };
            
            sql = "select * from " + _entity_sql + " where " + _sqlWhere + _sqlOrder;

            var dbSet = DbContextExtensions.exSet(this._appDbContext, _entity);
            var a = dbSet.FromSql(sql);


            return a;
        }

        public dynamic UpdateItem(dynamic item)

        {
            var updateRecord = item.record;
            var updateRecordId = System.Convert.ToInt32(item.p_recId);

            var ctx = this._appDbContext;

            //get entity stub
            var assembly = Assembly.GetExecutingAssembly();

            Type type = ctx.GetType().Assembly.GetExportedTypes().FirstOrDefault(t => t.Name == item.p_entity);

            //var type = assembly.GetTypes()
            //    .First(t => t.Name == item.p_entity);
            var entityStub = Activator.CreateInstance(type);


            dynamic typedEntityRecord = GetUpdateAddObject(updateRecord, entityStub);

            var contextWithEntity = DbContextExtensions.exSet2(this._appDbContext, item.p_entity);

            dynamic updateRecReturn;
            if (item.p_recId == "0")
            {
                updateRecReturn = contextWithEntity.Add(typedEntityRecord);

            }
            else
            {
                updateRecReturn = contextWithEntity.Update(typedEntityRecord);
            }

            ctx.SaveChanges();
            ctx.Dispose();

            var e = updateRecReturn.Entity;
            return e;

        }
        public dynamic DeleteItem(dynamic item)

        {
            var updateRecord = item.record;

            var ctx = this._appDbContext;

            //get entity stub
            var assembly = Assembly.GetExecutingAssembly();
            Type type = ctx.GetType().Assembly.GetExportedTypes().FirstOrDefault(t => t.Name == item.p_entity);


            //var type = assembly.GetTypes()
            //    .First(t => t.Name == item.p_entity);
            var entityStub = Activator.CreateInstance(type);

            dynamic typedEntityRecord = GetUpdateAddObject(updateRecord, entityStub);
            var contextWithEntity = DbContextExtensions.exSet2(this._appDbContext, item.p_entity);

            dynamic updateRecReturn;
            updateRecReturn = contextWithEntity.Remove(typedEntityRecord);

            ctx.SaveChanges();
            ctx.Dispose();

            var e = updateRecReturn.Entity;

            return e;

        }
        private dynamic GetUpdateAddObject(dynamic updateRecord, dynamic entityStub)
        {
            Newtonsoft.Json.Linq.JObject jobj3 = Newtonsoft.Json.Linq.JObject.FromObject(updateRecord, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
            System.Type returnType = entityStub.GetType();
            dynamic dd3 = jobj3.ToObject(returnType, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });

            return dd3;
        }
    }
}
