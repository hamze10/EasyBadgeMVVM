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
    public class RuleVM : IRuleVM
    {
        private int filterId;
        private Filter filter;  // filter
        private Field field;    // field choosen for the filter
        private ObservableCollection<Target> targets;
        private ObservableCollection<BadgeEvent> badgeEvents;

        private IDbEntities dbEntities;
        private ObservableCollection<Rule> rules;

        public RuleVM(int filterId)
        {
            this.filterId = filterId;
            this.dbEntities = new DbEntities();
        }

        public ObservableCollection<Rule> Rules
        {
            get
            {
                if (rules == null)
                    rules = dbEntities.GetAllRules(filterId);
                return rules;
            }
        }

        public Filter Filter
        {
            get
            {
                if (filter == null)
                    filter = dbEntities.GetAllFilters().FirstOrDefault(f => f.ID_Filter == filterId);
                return filter;
            }
        }

        public Field Field
        {
            get
            {
                if (field == null)
                {
                    int idField = dbEntities.GetEventFieldByEvent(Filter.EventFieldEventID_Event)
                        .FirstOrDefault(ef => ef.FieldID_Field == Filter.EventFieldFieldID_Field).FieldID_Field;
                    field = dbEntities.GetAllFields().FirstOrDefault(f => f.ID_Field == idField);
                }
                return field;
            }
        }

        public ObservableCollection<Target> Targets
        {
            get
            {
                if (targets == null)
                    targets = dbEntities.GetAllTargets();
                return targets;
            }
        }

        public ObservableCollection<BadgeEvent> BadgeEvents
        {
            get
            {
                //if (badgeEvents == null)
                    //badgeEvents = dbEntities.GetAllBadgeEvent(idEvent);
                return badgeEvents;
            }
        }

    }
}
