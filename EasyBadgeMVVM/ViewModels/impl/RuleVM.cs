﻿using EasyBadgeMVVM.Models;
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
        private Filter currentFilter;
        private Field field;
        private ObservableCollection<Target> targets;
        private ObservableCollection<BadgeEvent> badgeEvents;

        private IDbEntities dbEntities;
        private ObservableCollection<Rule> rules;

        public RuleVM(int filterId)
        {
            this.filterId = filterId;
            this.dbEntities = new DbEntities();
        }

        /// <summary>
        /// Insert the new rule to the collection (+ DB)
        /// </summary>
        public Rule SaveNewRule(Rule newRule)
        {
            dbEntities.InsertNewRule(newRule);
            dbEntities.SaveAllChanges();
            rules.Add(newRule);
            return dbEntities.GetAllRules(filterId).OrderBy(r => r.ID_Rule).Last();
        }

        /// <summary>
        /// Update all rules of the collection
        /// </summary>
        public void UpdateAllRules()
        {
            foreach (Rule item in rules)
            {
                dbEntities.UpdateRule(item.ID_Rule, item);
            }
            dbEntities.SaveAllChanges();
        }

        /// <summary>
        /// Delete the rule with the given Rule_ID
        /// </summary>
        public void DeleteRule(int ruleId)
        {
            dbEntities.DeleteRule(ruleId);
            dbEntities.SaveAllChanges();
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
                if (currentFilter == null)
                    currentFilter = dbEntities.GetAllFilters().FirstOrDefault(f => f.ID_Filter == filterId);
                return currentFilter;
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
                if (badgeEvents == null)
                {
                    badgeEvents = dbEntities.GetAllBadgeEvent();
                    /*
                    badgeEvents = new ObservableCollection<BadgeEvent>
                    {
                        new BadgeEvent
                        {
                            ID_BadgeEvent = 1,
                            EventID_Event = 1,
                            BadgeID_Badge = -1,
                            Name = "BadgeEventName1",
                            DefaultPrint = true
                        },
                        new BadgeEvent
                        {
                            ID_BadgeEvent = 2,
                            EventID_Event = 1,
                            BadgeID_Badge = -1,
                            Name = "BadgeEventName2",
                            DefaultPrint = false
                        },
                    };
                    */
                }
                return badgeEvents;
            }
        }

    }
}
