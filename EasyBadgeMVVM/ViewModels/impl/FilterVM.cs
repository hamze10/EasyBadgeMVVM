using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.ViewModels.interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.ViewModels.impl
{
    public class FilterVM : IFilterVM
    {
        private IDbEntities dbEntities;
        private ObservableCollection<Field> fields;
        private ObservableCollection<Filter> filters;
        private int eventId;

        public FilterVM(int eventId)
        {
            this.eventId = eventId;
            this.dbEntities = new DbEntities();
        }

        public ObservableCollection<Field> Fields
        {
            get
            {
                if (fields == null)
                    fields = dbEntities.GetAllFields();
                return fields;
            }
        }

        public ObservableCollection<Filter> Filters
        {
            get
            {
                if (filters == null)
                {
                    filters = dbEntities.GetAllFilters(eventId);
                }
                return filters;
            }
        }

        /// <summary>
        /// Insert the new filter to the collection (+ DB)
        /// </summary>
        public Filter SaveNewFilter(Filter newFilter)
        {
            dbEntities.InsertNewFilter(newFilter);
            dbEntities.SaveAllChanges();
            filters.Add(newFilter);
            return dbEntities.GetAllFilters(eventId).OrderBy(f => f.ID_Filter).Last();
        }

        /// <summary>
        /// Update all filters of the collection
        /// </summary>
        public void UpdateAllFilters()
        {
            foreach(Filter item in filters)
            {
                dbEntities.UpdateFilter(item.ID_Filter, item);
            }
            dbEntities.SaveAllChanges();
        }

        /// <summary>
        /// Delete the filter with the given Filter_ID
        /// </summary>
        public void DeleteFilter(int filterId)
        {
            dbEntities.DeleteFilter(filterId);
            dbEntities.SaveAllChanges();
        }
    }    
}
