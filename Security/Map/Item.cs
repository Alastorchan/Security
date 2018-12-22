using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Map
{
    /// <summary>
    /// Элемент словаря.
    /// </summary>
    public class Item<TKye, TValue>
    {
        /// <summary>
        /// Ключ.
        /// </summary>
        public TKye Key { get; set; }

        /// <summary>
        /// Значение.
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Создать новый пустой экземпляр класса Item.
        /// </summary>
        public Item() { }

        /// <summary>
        /// Создано новый экземпляр класса Item.
        /// </summary>
        /// <param name="key"> Ключ. </param>
        /// <param name="value"> Значение. </param>
        public Item(TKye key, TValue value)
        {
            // Проверяем входные данные на корректность.
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Key = key;
            Value = value;
        }

        /// <summary>
        /// Приведение объекта к строке.
        /// </summary>
        /// <returns> Хранимое значение. </returns>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
