using System;


namespace Horeb.Infrastructure.Data
{
    public interface IValueClass<T>
    {
        T Id { get; set;}
        Type GetIdType();
    }

    public abstract class ValueClass<V> : IValueClass<V>
    {
        private V _id;
        private bool _idHasBeenSet = false;

        public virtual V Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_idHasBeenSet)
                    ThrowExceptionIfOverwritingAnId();
                _id = value;
                _idHasBeenSet = true;
            }
        }

        public Type GetIdType()
        {
            return Id.GetType();
        }

        private void ThrowExceptionIfOverwritingAnId()
        {
            throw new ApplicationException("You cannot change the Id of an entity.");
        }
    }
}