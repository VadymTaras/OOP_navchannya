using System.ComponentModel;

namespace Lab04_DataGridView
{
    /// <summary>
    /// Модель сутності товару для відображення та редагування в DataGridView.
    /// Реалізує INotifyPropertyChanged для синхронізації змін у реальному часі.
    /// </summary>
    public class ProductItem : INotifyPropertyChanged
    {
        private int _id;
        private string _name = string.Empty;
        private string _category = "Електроніка";
        private decimal _price;
        private int _quantity;
        private bool _inStock = true;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                    OnPropertyChanged(nameof(TotalValue));
                }
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(TotalValue));
                }
            }
        }

        public bool InStock
        {
            get => _inStock;
            set
            {
                if (_inStock != value)
                {
                    _inStock = value;
                    OnPropertyChanged(nameof(InStock));
                }
            }
        }

        public decimal TotalValue => _price * _quantity;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
