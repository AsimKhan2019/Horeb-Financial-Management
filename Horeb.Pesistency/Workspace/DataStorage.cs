using Horeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Horeb.Persistency.Text
{
    public class DataStorageWithTypeId<TId> where TId : IEquatable<TId>
    {
        public Dictionary<string, Dictionary<TId, object>> Items { get; set; }
        public Dictionary<string, TId> Identities { get; set; }

        public DataStorageWithTypeId()
        {
            Items = new Dictionary<string, Dictionary<TId, object>>();
            Identities = new Dictionary<string, TId>();
        }

        internal bool ContainsKey(Type key)
        {
            return Items.ContainsKey(key.FullName);
        }

        internal Dictionary<TId, object> GetDataList(Type key)
        {
            if (!Items.ContainsKey(key.FullName))
            {
                var subList = new Dictionary<TId, object>();
                Items.Add(key.FullName, subList);
            }
            return Items[key.FullName];
        }

        internal void Clear()
        {
            Items.Clear();
            Identities.Clear();
        }

        internal TObject GetById<TObject>(TId id)
        {
            Dictionary<TId, object> list = GetDataList(typeof(TObject));
            if (list.ContainsKey(id)) return (TObject)list[id];
            return default(TObject);
        }

        internal IEnumerable<TObject> GetItems<TObject>()
        {
            Dictionary<TId, object> list = GetDataList(typeof(TObject));
            return list.Values.Cast<TObject>();
        }

        internal void Delete<TObject>(TId id)
        {
            Dictionary<TId, object> list = GetDataList(typeof(TObject));
            list.Remove(id);
        }

        internal void Add(object obj)
        {
            var list = GetDataList(obj.GetType());
            var idt = obj as IValueClass<TId>;
            if (idt != null && idt.Id != null)
            {
                if (list.ContainsKey(idt.Id))
                    Update(idt);
                else list.Add(idt.Id, idt);
            }
            else
            {
                if (list.ContainsKey(default(TId))) Update(obj);
                else list.Add(default(TId), obj);
            }
        }

        internal void Update(object obj)
        {
            var list = GetDataList(obj.GetType());

            var idt = obj as IValueClass<TId>;

            if (idt != null)
            {
                if (idt.Id.Equals(default(TId)))
                {
                    Add(idt);
                    return;
                }
                if (list.ContainsKey(idt.Id))
                    list[idt.Id] = idt;
            }
            else if (list.ContainsKey(default(TId)))
            {
                list[default(TId)] = obj;
            }
            else list.Add(default(TId), obj);
        }

        internal IList<TObject> GetItems<TObject>(Expression<Func<TObject, bool>> predicate)
        {
            return GetDataList(typeof(TObject)).Values.Cast<TObject>().Where(predicate.Compile()).ToList();
        }



    }
}