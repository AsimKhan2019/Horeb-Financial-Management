using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Horeb.Domain.UserModule
{
    /// <summary>A collection of <see cref="T:Horeb.Domain.UserModule.HorebUser" /> objects.</summary>
    [Serializable]
    public sealed class HorebUserCollection : IEnumerable, ICollection
    {
        private Hashtable _Indices;
        private ArrayList _Values;
        private bool _ReadOnly;

        /// <summary>Creates a new, empty  user  collection.</summary>  
        public HorebUserCollection()
        {
            this._Indices = new Hashtable(10, (IEqualityComparer)StringComparer.CurrentCultureIgnoreCase);
            this._Values = new ArrayList();
        }

        /// <summary>Adds the specified  user  to the collection.</summary>
        /// <param name="user">A <see cref="T:Horeb.Domain.UserModule.HorebUser" /> object to add to the collection.</param>
        /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <see cref="P:Horeb.Domain.UserModule.HorebUser.Id" /> of the <paramref name="user" /> is null or empty.</exception>
        /// <exception cref="T:System.ArgumentException">A <see cref="T:Horeb.Domain.UserModule.HorebUser" /> object with the same <see cref="P:Horeb.Domain.UserModule.HorebUser.Id" /> value as <paramref name="user" /> already exists in the collection.</exception>
        public void Add(HorebUser user)
        {
            if (user == null || user == HorebUser.Empty)
                throw new ArgumentNullException(nameof(user));
            if (this._ReadOnly)
                throw new NotSupportedException();
            int index = this._Values.Add((object)user);
            try
            {
                this._Indices.Add((object)user.Id, (object)index);
            }
            catch
            {
                this._Values.RemoveAt(index);
                throw;
            }
        }

        /// <summary>Removes the  user  object with the specified user Id number from the collection.</summary>
        /// <param name="userId">The user Id of the <see cref="T:Horeb.Domain.UserModule.HorebUser" /> object to remove from the collection.</param>
        /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
        public void Remove(string userId)
        {
            if (this._ReadOnly)
                throw new NotSupportedException();
            object index1 = this._Indices[(object)userId];
            if (index1 == null)
                return;
            int index2 = (int)index1;
            if (index2 >= this._Values.Count)
                return;
            this._Values.RemoveAt(index2);
            this._Indices.Remove((object)userId);
            ArrayList arrayList = new ArrayList();
            foreach (DictionaryEntry index3 in this._Indices)
            {
                if ((int)index3.Value > index2)
                    arrayList.Add(index3.Key);
            }
            foreach (string str in arrayList)
                this._Indices[(object)str] = (object)((int)this._Indices[(object)str] - 1);
        }

        /// <summary>Gets the  user  in the collection referenced by the specified user id.</summary>
        /// <returns>A <see cref="T:Horeb.Domain.Models.HorebUserHorebUser" /> object representing the user specified by <paramref name="userId" />.</returns>
        /// <param name="userId">The <see cref="P:Horeb.Domain.Models.HorebUserHorebUser.Id" /> of the <see cref="T:Horeb.Domain.Models.HorebUserHorebUser" /> to retrieve from the collection.</param>
        public HorebUser this[string userId]
        {
            get
            {
                object index1 = this._Indices[(object)userId];
                if (index1 == null)
                    return (HorebUser)null;
                int index2 = (int)index1;
                if (index2 >= this._Values.Count)
                    return (HorebUser)null;
                return (HorebUser)this._Values[index2];
            }
        }

        /// <summary>Gets an enumerator that can iterate through the  user  collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the entire <see cref="T:Horeb.Domain.UserModule.HorebUserCollection" />.</returns>
        public IEnumerator GetEnumerator()
        {
            return this._Values.GetEnumerator();
        }

        /// <summary>Makes the contents of the  user  collection read-only.</summary>
        public void SetReadOnly()
        {
            if (this._ReadOnly)
                return;
            this._ReadOnly = true;
            this._Values = ArrayList.ReadOnly(this._Values);
        }

        /// <summary>Removes all  user  objects from the collection.</summary>
        public void Clear()
        {
            this._Values.Clear();
            this._Indices.Clear();
        }

        /// <summary>Gets the number of  user  objects in the collection.</summary>
        /// <returns>The number of <see cref="T:Horeb.Domain.UserModule.HorebUser" /> objects in the collection.</returns>
        public int Count
        {
            get
            {
                return this._Values.Count;
            }
        }

        /// <summary>Gets a value indicating whether the  user  collection is thread safe.</summary>
        /// <returns>Always false because thread-safe  user  collections are not supported.</returns>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>Gets the synchronization root.</summary>
        /// <returns>Always this, because synchronization of  user  collections is not supported.</returns>
        public object SyncRoot
        {
            get
            {
                return (object)this;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this._Values.CopyTo(array, index);
        }

        /// <summary>Copies the  user  collection to a one-dimensional array.</summary>
        /// <param name="array">A one-dimensional array of type <see cref="T:Horeb.Domain.UserModule.HorebUser" /> that is the destination of the elements copied from the <see cref="T:Horeb.Domain.UserModule.HorebUserCollection" />. The array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(HorebUser[] array, int index)
        {
            this._Values.CopyTo((Array)array, index);
        }
    }
}
