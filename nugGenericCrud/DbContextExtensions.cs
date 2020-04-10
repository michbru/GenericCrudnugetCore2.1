using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nugGenericCrud
{
    public static class DbContextExtensions
    {
        public static IQueryable<Object> exSet(this DbContext _context, Type t)
        {
            return (IQueryable<Object>)_context.GetType().GetMethod("Set").MakeGenericMethod(t).Invoke(_context, null);
        }
        public static IQueryable<Object> exSet(this DbContext _context, String table)
        {
            Type TableType = _context.GetType().Assembly.GetExportedTypes().FirstOrDefault(t => t.Name == table);
            IQueryable<Object> ObjectContext = _context.exSet(TableType);
            return ObjectContext;
        }
        public static dynamic exSet2(this DbContext _context, String table)
        {
            Type TableType = _context.GetType().Assembly.GetExportedTypes().FirstOrDefault(t => t.Name == table);
            var ObjectContext = _context.exSet(TableType);
            return ObjectContext;
        }
    }
}
