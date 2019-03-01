﻿using EasyBadgeMVVM.Models;


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace EasyBadgeMVVM.ViewModels
{
    public interface IDbEntities
    {
        ObservableCollection<UserEventDTO> GetAllUsers();
        void InsertNewField(string field);
        void InsertNewUser(int index, string field, string data);
        void SetIdEvent(int idEvent);
        void SaveAllChanges();

        /// <summary>
        /// Check if the user with the last name and first name in parameter exists
        /// </summary>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <returns>true if existe, otherwhise false</returns>
        bool CheckIfAlreadyExists(string lastName, string firstName);

        void Clear();
        ObservableCollection<Event> GetEvents();
        Event GetEventById(int idEvent);
        Event SearchFor(Expression<Func<Event, bool>> predicate);
        bool InsertInEventTable(Event ev);
    }
}