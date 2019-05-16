using EasyBadgeMVVM.Filters;
using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.ViewModels
{
    public class UserVM : INotifyPropertyChanged, IUserVM
    {
        /*********************************************************************************************************************************************************************/
        /****** CONSTRUCTOR ******/
        /*********************************************************************************************************************************************************************/

        private int _idEvent;
        private IDbEntities _dbEntities;

        public UserVM(int idEvent)
        {
            this._dbEntities = new DbEntities();
            this._idEvent = idEvent;
            this._dbEntities.SetIdEvent(this._idEvent);
        }

        public int IdEvent
        {
            get
            {
                return this._idEvent;
            }
        }

        /*********************************************************************************************************************************************************************/
        /****** PROPERTY CHANGED ******/
        /*********************************************************************************************************************************************************************/

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /*********************************************************************************************************************************************************************/
        /****** MAIN ******/
        /*********************************************************************************************************************************************************************/

        int i = 0;
        public void InsertNewUser(Dictionary<string, string> values)
        {
            foreach(KeyValuePair<string, string> entry in values)
            {
                string field = entry.Key.Trim().Split(':')[0].Trim();
                this._dbEntities.InsertNewUser(i, field, entry.Value, this._dbEntities.GetVisibilityField(field));
                i++;
            }
            this._dbEntities.SaveAllChanges();
        }

        public EventSet GetEventById(int idEvent)
        {
            return this._dbEntities.GetEventById(idEvent);
        }



        /*********************************************************************************************************************************************************************/
        /****** FILTERS ******/
        /*********************************************************************************************************************************************************************/
        
        private enum FieldTypes { Characters, Numbers };

        /// <summary>
        /// Determine if the given user match the defined filters/rules.
        /// If that's the case, the UI should display the define color.
        /// 
        /// Comparison can be currently done on : double, string.
        /// You can then compare with the name, the company name, the age...
        /// The following ones could be added in a future version : bool, datetime
        /// </summary>
        /// <returns>Returns the hexadecimal code, null if there is no filter that match.</returns>
        public string DetermineColorForCard(List<EventFieldUserSet> currentUser)
        {
            string hexaDecimalToReturn = null;
            for(int i = 0; i < currentUser.Count; i++)
            {
                string fieldName = currentUser[i].EventFieldSet.FieldSet.Name;
                string fieldValue = currentUser[i].Value;

                // Determine the type (numbers and characters are take over)
                Regex numbersRegex = new Regex(@"^[0-9]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (numbersRegex.IsMatch(fieldValue))
                {
                    // Type = NUMBERS
                    List<FilterSet> relevantFilters = _dbEntities.GetAllFilters(IdEvent)
                        .Where(f => FiltersHelper.LogicalOperatorsForNumbers.Contains(f.LogicalOperator)
                        && f.EventFieldSet.FieldSet.Name.Equals(fieldName))
                        .ToList();
                    foreach (FilterSet filter in relevantFilters)
                    {
                        if (FiltersHelper.Evaluate(Convert.ToDouble(fieldValue), Convert.ToDouble(filter.Value), filter.LogicalOperator))
                        {
                            // There is a MATCH - check if a rule is defined for "Window" target
                            return RetrieveWindowColor(filter.ID_Filter);
                        }
                    }
                }
                else
                {
                    // Type = CHARACTERS
                    List<FilterSet> relevantFilters = _dbEntities.GetAllFilters(IdEvent)
                        .Where(f => FiltersHelper.LogicalOperatorsForCharacters.Contains(f.LogicalOperator)
                        && f.EventFieldSet.FieldSet.Name.Equals(fieldName))
                        .ToList();
                    foreach (FilterSet filter in relevantFilters)
                    {
                        if (FiltersHelper.Evaluate(fieldValue, filter.Value, filter.LogicalOperator))
                        {
                            // There is a MATCH - check if a rule is defined for "Window" target
                            return RetrieveWindowColor(filter.ID_Filter);
                        }
                    }
                }
            }
            return hexaDecimalToReturn;
        }

        /// <summary>
        /// Returns the hexa code defined for the "Window" target, if there is one.
        /// Returns null otherwise
        /// </summary>
        private string RetrieveWindowColor(int iD_Filter)
        {
            RuleSet windowRule = _dbEntities.GetAllRules(iD_Filter).FirstOrDefault(r => r.TargetSet.Name.Equals("Window"));
            return windowRule != null ? windowRule.HexaCode : null;            
        }
    }
}
