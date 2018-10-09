//------------------------------------------------------------
// <copyright file="CollectionViewModelBase.cs" company="CyberFeedForward" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Common.WPF
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// For use with a viewmodel, whose primary concern is
    /// managing a collection of items, to be displayed in categories.
    /// </summary>
    /// <typeparam name="TItem">Viewmodel for items</typeparam>
    /// <typeparam name="TCategory">Viewmodel for categories</typeparam>
    public abstract class CollectionModelBase<TItem, TCategory> :
        ViewModelBase
        where TCategory : ICategoryModel
        where TItem : IItemModel<TCategory>
    {
        /// <summary>
        /// Event raised when items are added or removed from collection
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public CollectionModelBase()
        {
            hashItems = new Hashtable();
            Categories = new ObservableCollection<TCategory>();
            Items = new ObservableCollection<TItem>();
            Items.CollectionChanged += (s, e) =>
            {
                if (NotifyItemsByCategory)
                {
                    NotifyPropertyUpdated(nameof(ItemsByCategory));
                    CollectionChanged?.Invoke(s, e);
                }

                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (TItem item in e.NewItems)
                        hashItems.Add(item.ID, item);
                }
                else if (e.Action == NotifyCollectionChangedAction.Move)
                {
                    foreach (TItem item in e.NewItems)
                        hashItems.Remove(item.ID);
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (TItem item in e.OldItems)
                        hashItems.Remove(item.ID);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    foreach (TItem item in e.NewItems)
                        hashItems.Add(item.ID, item);

                    foreach (TItem item in e.OldItems)
                        hashItems.Remove(item.ID);
                }
                else if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    hashItems.Clear();
                }
            };
            SearchTerm = string.Empty;
        }

        /// <summary>
        /// Notify collection has changed.
        /// </summary>
        protected void RaiseCollectionChange(object s, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(s, e);
        }

        #region Items
        /// <summary>
        /// Gets all items stored
        /// </summary>
        [NotMapped]
        public ObservableCollection<TItem> Items { get; private set; }
        
        private Hashtable hashItems;

        /// <summary>
        /// Add item to collection
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="ignoreDuplicates">If true, ignore duplicates</param>
        public void Add(TItem item, bool ignoreDuplicates = true)
        {
            if (!hashItems.ContainsKey(item.ID))
            {
                Items.Add(item);
            }
            else if (!ignoreDuplicates)
            {
                throw new System.ArgumentException("Duplicate key");
            }
        }

        public void Remove(int id)
        {
            if (hashItems.ContainsKey(id))
            {
                Items.Remove((TItem)hashItems[id]);
            }
        }

        /// <summary>
        /// Clear all items from list
        /// </summary>
        public void Clear()
        {
            Items.Clear();
            Categories.Clear();
        }

        public TItem GetItem(int itemID)
        {
            if (hashItems.ContainsKey(itemID))
                return (TItem)hashItems[itemID];
            else
                throw new KeyNotFoundException();
        }

        public bool Contains(int itemID)
        {
            if (hashItems.ContainsKey(itemID))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets grouping. Default: items are grouped by category and ordered by title
        /// </summary>
        [NotMapped]
        public IOrderedEnumerable<IGrouping<TCategory, TItem>> ItemsByCategory
        {
            get
            {
                return GetState<IOrderedEnumerable<IGrouping<TCategory, TItem>>>();
            }

            private set
            {
                SetState(value);
            }
        }
        #endregion

        #region Categories        
        /// <summary>
        /// Gets or sets whether we notify callers of ItemsByCategory
        /// when Items have changed. Disable to load data to Items.
        /// </summary>
        [NotMapped]
        public bool NotifyItemsByCategory
        {
            get
            {
                return GetState(true);
            }

            set
            {
                SetState(value);
                if (value)
                {
                    NotifyPropertyUpdated(nameof(ItemsByCategory));
                }
            }
        }

        /// <summary>
        /// Gets categories used to group items
        /// </summary>
        [NotMapped]
        public ObservableCollection<TCategory> Categories { get; }

        /// <summary>
        /// Get category associated with category name
        /// </summary>
        public abstract TCategory GetCategory(string catName);
        #endregion

        /// <summary>
        /// Gets or sets search term
        /// </summary>
        [NotMapped]
        public string SearchTerm
        {
            get
            {
                return GetState(string.Empty);
            }

            set
            {
                if (SetState(value))
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        ItemsByCategory = from item in Items
                                          orderby item.PageTitle ascending
                                          group item by item.Category into g
                                          orderby g.Key.Title ascending
                                          select g;
                    }
                    else
                    {
                        var upperSearchTerm = SearchTerm.ToUpper();
                        ItemsByCategory = from item in Items
                                          where item.PageTitle.ToUpper().Contains(upperSearchTerm)
                                          orderby item.PageTitle ascending
                                          group item by item.Category into g
                                          orderby g.Key.Title ascending
                                          select g;
                    }

                    CollectionChanged?.Invoke(
                        this,
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Reset));
                }
            }
        }
    }
}
