using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Map
{
    /// <summary>
    /// Словарь.
    /// </summary>
    public class Map<TKey, TValue>
    {
        /// <summary>
        /// Коллекция хранимых данных.
        /// </summary>
        private List<Item<TKey, TValue>> _items = new List<Item<TKey, TValue>>();

        /// <summary>
        /// Количество элементов.
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Коллекция ключей.
        /// </summary>
        public IReadOnlyList<TKey> Keys => (IReadOnlyList<TKey>)_items.Select(i => i.Key).ToList();

        /// <summary>
        /// Добавить данные в словарь.
        /// </summary>
        /// <param name="item"> Элемент добавляемых данных в виде пары ключ-значение. </param>
        public void Add(Item<TKey, TValue> item)
        {
            // Проверяем входные данные на корректность.
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (_items.Any(i => i.Key.Equals(item.Key)))
            {
                throw new ArgumentException($"Словарь уже содержит значение с ключом {item.Key}.", nameof(item));
            }

            // Добавляем данные в коллекцию.
            _items.Add(item);
        }

        /// <summary>
        /// Добавить данные в словарь.
        /// </summary>
        /// <param name="key"> Ключ, по которому доступны хранимые данные. </param>
        /// <param name="value"> Данные, хранимые в словаре. </param>
        public void Add(TKey key, TValue value)
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

            if (_items.Any(i => i.Key.Equals(key)))
            {
                throw new ArgumentException($"Словарь уже содержит значение с ключом {key}.", nameof(key));
            }

            // Создаем новый элемент хранимых данных.
            var item = new Item<TKey, TValue>()
            {
                Key = key,
                Value = value
            };

            // Добавляем данные в коллекцию.
            _items.Add(item);
        }

        /// <summary>
        /// Удалить данные из коллекции по ключу.
        /// </summary>
        /// <param name="key"> Ключ, по которому доступны данные. </param>
        public void Remove(TKey key)
        {
            // Проверяем входные данные на корректность.
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            // Получаем элемент данных из коллекции по ключу.
            var item = _items.SingleOrDefault(i => i.Key.Equals(key));

            // Если данные найдены по ключу, 
            // то удаляем их из коллекции.
            if (item != null)
            {
                _items.Remove(item);
            }
        }

        /// <summary>
        /// Изменить хранимые данные по ключу.
        /// </summary>
        /// <param name="key"> Ключ, по которому доступны хранимые данные. </param>
        /// <param name="newValue"> Новое значение данных. </param>
        public void Update(TKey key, TValue newValue)
        {
            // Проверяем входные данные на корректность.
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (newValue == null)
            {
                throw new ArgumentNullException(nameof(newValue));
            }

            if (!_items.Any(i => i.Key.Equals(key)))
            {
                throw new ArgumentException($"Словарь не содержит значение с ключом {key}.", nameof(key));
            }

            // Получаем элемент данных по ключу.
            var item = _items.SingleOrDefault(i => i.Key.Equals(key));

            // Если данные найдены по ключу, 
            // то изменяем хранимое значение на новое.
            if (item != null)
            {
                item.Value = newValue;
            }
        }

        /// <summary>
        /// Получит значение по ключу.
        /// </summary>
        /// <param name="key"> Ключ, по которому доступны хранимые данные. </param>
        /// <returns> Значение хранимых данных. </returns>
        public TValue Get(TKey key)
        {
            // Проверяем входные данные на корректность.
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            // Получаем значение по ключу.
            var item = _items.SingleOrDefault(i => i.Key.Equals(key)) ??
                throw new ArgumentException($"Словарь не содержит значение с ключом {key}.", nameof(key));

            // Возвращаем значение хранимых данных.
            return item.Value;
        }
    }
}
