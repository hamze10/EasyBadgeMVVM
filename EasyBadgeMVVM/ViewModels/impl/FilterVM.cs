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
        private ObservableCollection<FieldSet> fields;
        private ObservableCollection<FilterSet> filters;
        private int eventId;

        public FilterVM(int eventId)
        {
            this.eventId = eventId;
            this.dbEntities = new DbEntities();
            this.dbEntities.SetIdEvent(eventId);
        }

        public ObservableCollection<FieldSet> Fields
        {
            get
            {
                if (fields == null)
                    fields = dbEntities.GetAllFields();
                return fields;
            }
        }

        public ObservableCollection<FilterSet> Filters
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
        public FilterSet SaveNewFilter(FilterSet newFilter)
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
            foreach(FilterSet item in filters)
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
