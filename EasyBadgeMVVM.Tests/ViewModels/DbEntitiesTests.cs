using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.ViewModels;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using System;
using System.Collections.ObjectModel;

namespace EasyBadgeMVVM.Tests.ViewModels
{
    [TestClass]
    public class DbEntitiesTests
    {
        /*******************************************************
         * ****************** GET ******************************
         * ****************************************************/

        private IDbEntities dbEntitiesMock = Mock.Of<IDbEntities>();

        /* GET ALL FIELDS */

        [TestMethod]
        public void GetAllFields_ReturnEmpty()
        {
            ObservableCollection<FieldSet> fakeFields = new ObservableCollection<FieldSet>();

            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetAllFields()).Returns(fakeFields);

            ObservableCollection<FieldSet> allFields = dbEntitiesMock.GetAllFields();
            Assert.AreEqual(0, allFields.Count);
        }

        [TestMethod]
        public void GetAllFields_Return1()
        {
            ObservableCollection<FieldSet> fakeFields = new ObservableCollection<FieldSet>(new FieldSet[]
            {
                new FieldSet
                {
                    ID_Field = 1,
                    Name = "FirstName"
                }
            });

            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetAllFields()).Returns(fakeFields);

            ObservableCollection<FieldSet> allFields = dbEntitiesMock.GetAllFields();
            Assert.AreEqual(1, allFields.Count);
        }

        [TestMethod]
        public void GetAllFields_Return2()
        {
            ObservableCollection<FieldSet> fakeFields = new ObservableCollection<FieldSet>(new FieldSet[]
            {
                new FieldSet
                {
                    ID_Field = 1,
                    Name = "FirstName"
                },
                new FieldSet
                {
                    ID_Field = 2,
                    Name = "LastName"
                }
            });

            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetAllFields()).Returns(fakeFields);

            ObservableCollection<FieldSet> allFields = dbEntitiesMock.GetAllFields();
            Assert.AreEqual(2, allFields.Count);
        }

        /* GET ALL USERS */

        [TestMethod]
        public void GetAllUsers_ReturnEmpty()
        {
            ObservableCollection<UserSet> fakeUsers = new ObservableCollection<UserSet>();

            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetAllUsersSet()).Returns(fakeUsers);

            ObservableCollection<UserSet> allUsers = dbEntitiesMock.GetAllUsersSet();
            Assert.AreEqual(0, allUsers.Count);
        }

        [TestMethod]
        public void GetAllUsers_Return1()
        {
            ObservableCollection<UserSet> fakeUsers = new ObservableCollection<UserSet>(new UserSet[]
            {
                new UserSet
                {
                    ID_User = 1,
                    CreationDate = new DateTime(),
                    Active = true,
                    Barcode = "100000",
                    Onsite = false
                }
            });

            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetAllUsersSet()).Returns(fakeUsers);

            ObservableCollection<UserSet> allUsers = dbEntitiesMock.GetAllUsersSet();
            Assert.AreEqual(1, allUsers.Count);
        }

        [TestMethod]
        public void GetAllUsers_Return2()
        {
            ObservableCollection<UserSet> fakeUsers = new ObservableCollection<UserSet>(new UserSet[]
            {
                new UserSet
                {
                    ID_User = 1,
                    CreationDate = new DateTime(),
                    Active = true,
                    Barcode = "100000",
                    Onsite = false
                },
                new UserSet
                {
                    ID_User = 2,
                    CreationDate = new DateTime(),
                    Active = true,
                    Barcode = "100001",
                    Onsite = false
                }
            });

            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetAllUsersSet()).Returns(fakeUsers);

            ObservableCollection<UserSet> allUsers = dbEntitiesMock.GetAllUsersSet();
            Assert.AreEqual(2, allUsers.Count);
        }

        /* GET ALL EVENTS */

        [TestMethod]
        public void GetAllEvents_ReturnEmpty()
        {
            ObservableCollection<EventSet> fakeEvents = new ObservableCollection<EventSet>();

            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetEvents()).Returns(fakeEvents);

            ObservableCollection<EventSet> allEvents = dbEntitiesMock.GetEvents();
            Assert.AreEqual(0, allEvents.Count);
        }

        [TestMethod]
        public void GetAllEvents_Return1()
        {
            ObservableCollection<EventSet> fakeEvents = new ObservableCollection<EventSet>(new EventSet[]
            {
                new EventSet
                {
                    ID_Event = 1,
                    Name = "MyEv 1",
                    DateOfEvent = DateTime.Now
                }
            });

            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetEvents()).Returns(fakeEvents);

            ObservableCollection<EventSet> allEvents = dbEntitiesMock.GetEvents();
            Assert.AreEqual(1, allEvents.Count);
        }

        [TestMethod]
        public void GetAllEvents_Return2()
        {
            ObservableCollection<EventSet> fakeEvents = new ObservableCollection<EventSet>(new EventSet[]
            {
                new EventSet
                {
                    ID_Event = 1,
                    Name = "MyEv 1",
                    DateOfEvent = DateTime.Now
                },
                new EventSet
                {
                    ID_Event = 2,
                    Name = "MyEv 2",
                    DateOfEvent = DateTime.Now
                }
            });

            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetEvents()).Returns(fakeEvents);

            ObservableCollection<EventSet> allEvents = dbEntitiesMock.GetEvents();
            Assert.AreEqual(2, allEvents.Count);
        }

        /* GET ALL BADGES */

        [TestMethod]
        public void GetAllBadges_ReturnEmpty()
        {
            ObservableCollection<BadgeSet> fakeBadges = new ObservableCollection<BadgeSet>();

            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetAllBadges()).Returns(fakeBadges);

            ObservableCollection<BadgeSet> allBadges = dbEntitiesMock.GetAllBadges();
            Assert.AreEqual(0, allBadges.Count);
        }

        [TestMethod]
        public void GetAllBadges_Return1()
        {
            ObservableCollection<BadgeSet> fakeBadges = new ObservableCollection<BadgeSet>(new BadgeSet[]
            {
                new BadgeSet
                {
                    ID_Badge = 1,
                    Dimension_X = 97,
                    Dimension_Y = 86,
                    TypeBadge = "A5",
                    Name = "Butterfly"
                }
            });

            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetAllBadges()).Returns(fakeBadges);

            ObservableCollection<BadgeSet> allBadges = dbEntitiesMock.GetAllBadges();
            Assert.AreEqual(1, allBadges.Count);
        }

        [TestMethod]
        public void GetAllBadges_Return2()
        {
            ObservableCollection<BadgeSet> fakeBadges = new ObservableCollection<BadgeSet>(new BadgeSet[]
            {
                new BadgeSet
                {
                    ID_Badge = 1,
                    Dimension_X = 97,
                    Dimension_Y = 86,
                    TypeBadge = "A5",
                    Name = "Butterfly"
                },
                new BadgeSet
                {
                    ID_Badge = 2,
                    Dimension_X = 97,
                    Dimension_Y = 116,
                    TypeBadge = "XL",
                    Name = "Butterfly"
                }
            });

            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetAllBadges()).Returns(fakeBadges);

            ObservableCollection<BadgeSet> allBadges = dbEntitiesMock.GetAllBadges();
            Assert.AreEqual(2, allBadges.Count);
        }

        /* GET ALL EVENTFIELD */

        [TestMethod]
        public void GetAllEventField_Return2Event1()
        {
            ObservableCollection<EventFieldSet> fakeEventField = new ObservableCollection<EventFieldSet>(new EventFieldSet[]
            {
                new EventFieldSet
                {
                    FieldSet = new FieldSet
                    {
                        ID_Field = 1,
                        Name = "LastName"
                    },

                    EventSet = new EventSet
                    {
                        ID_Event = 1,
                        Name = "MyEv",
                        DateOfEvent = DateTime.Now
                    },

                    Visibility = true,
                    Unique = false
                },

                new EventFieldSet
                {
                    FieldSet = new FieldSet
                    {
                        ID_Field = 2,
                        Name = "FirstName"
                    },

                    EventSet = new EventSet
                    {
                        ID_Event = 1,
                        Name = "MyEv",
                        DateOfEvent = DateTime.Now
                    },

                    Visibility = true,
                    Unique = false
                }
            });
            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetEventFieldByEvent(1)).Returns(fakeEventField);
            ObservableCollection<EventFieldSet> allEventField = dbEntitiesMock.GetEventFieldByEvent(1);
            Assert.AreEqual(2, allEventField.Count);
        }

        [TestMethod]
        public void GetAllEventField_Return1Event2()
        {
            ObservableCollection<EventFieldSet> fakeEventField = new ObservableCollection<EventFieldSet>(new EventFieldSet[]
            {
                new EventFieldSet
                {
                    FieldSet = new FieldSet
                    {
                        ID_Field = 1,
                        Name = "LastName"
                    },

                    EventSet = new EventSet
                    {
                        ID_Event = 2,
                        Name = "MyEv2",
                        DateOfEvent = DateTime.Now
                    },

                    Visibility = true,
                    Unique = false
                }
            });
            Mock.Get(dbEntitiesMock).Setup(mock => mock.GetEventFieldByEvent(2)).Returns(fakeEventField);
            ObservableCollection<EventFieldSet> allEventField = dbEntitiesMock.GetEventFieldByEvent(2);
            Assert.AreEqual(1, allEventField.Count);
        }
    }
}
