using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Common
{
    public class EmailAddress
    {
        private string emailAddress;
        public EmailAddress(string emailAddress)
        {
            this.emailAddress = (string.IsNullOrEmpty(emailAddress) == false) ? emailAddress : throw new ArgumentNullException(emailAddress);
        }

        public override string ToString()
        {
            return emailAddress;
        }

        public static implicit operator string(EmailAddress x)
        {
            if (x != null)
                return x.emailAddress;
            return string.Empty;
        }
    }
    public interface IEmailAddressProvider
    {
        IEnumerable<EmailAddress> GetEmailAddresses(Person x);
        IEnumerable<Person> GetAllPeopleWithContact();
        bool ShouldBeContacted(Person person);
        void SetContactStatus(Person x, bool shouldBeContacted);
        void Save();
    }

    public class JsonFileEmailAddressProvider : IEmailAddressProvider
    {
        public class Key
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime Birthday { get; set; }

            public static Key CreateFromPerson(Person x)
            {
                return new Key
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Birthday = x.BirthDay
                };
            }

            public override bool Equals(object obj)
            {
                if (obj is null)
                {
                    return false;
                }
                if (obj.GetType() == typeof(Key))
                {
                    var tmpObj = (Key)obj;
                    if ((string.Compare(FirstName, tmpObj.FirstName) == 0) &&
                        (string.Compare(LastName, tmpObj.LastName) == 0) &&
                        (Birthday == tmpObj.Birthday))
                    {
                        return true;
                    }
                }

                return false;
            }

            public override int GetHashCode()
            {
                return 0;
            }

        }
        public class ContactEntry
        {
            public Key Key { get; set; }
            public string[] Emails { get; set; }
            public bool ShouldBeContacted { get; set; }
        }

        private IDictionary<Key, ContactEntry> contacts;
        private readonly string pathToFamilyContacts;
        public JsonFileEmailAddressProvider(string pathToFamilyContacts)
        {
            contacts = new Dictionary<Key, ContactEntry>();
            var contactsRaw = JsonConvert.DeserializeObject<ContactEntry[]>(File.ReadAllText(pathToFamilyContacts));
            foreach(var contact in contactsRaw)
            {
                contacts.Add(contact.Key, contact);
            }
            this.pathToFamilyContacts = pathToFamilyContacts;
        }

        public bool ShouldBeContacted(Person x)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            var key = Key.CreateFromPerson(x);
            if (contacts.ContainsKey(key))
            {
                return contacts[key].ShouldBeContacted;
            }
            throw new ApplicationException($"Cound not find {x} in contact list.");
        }

        public void SetContactStatus(Person x, bool shouldBeContacted)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            var key = Key.CreateFromPerson(x);
            if (contacts.ContainsKey(key))
            {
                contacts[key].ShouldBeContacted = shouldBeContacted;
                return;
            }
            throw new ApplicationException($"Cound not find {x} in contact list.");
        }

        public void Save()
        {
            using (StreamWriter file = File.CreateText(pathToFamilyContacts))
            using (var writer = new JsonTextWriter(file))
            {
                writer.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();

                //serialize object directly into file stream
                serializer.Serialize(writer, contacts.Values);
            }
        }
        public IEnumerable<EmailAddress> GetEmailAddresses(Person x)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            var key = Key.CreateFromPerson(x);
            if (contacts.ContainsKey(key))
            {
                var contactEmails = new List<EmailAddress>();
                foreach (var email in contacts[key].Emails)
                {
                    contactEmails.Add(new EmailAddress(email));

                }
                return contactEmails;
            }
            throw new ApplicationException($"Cound not find {x} in contact list.");
        }

        public IEnumerable<Person> GetAllPeopleWithContact()
        {
            var people = new List<Person>();
            foreach(var key in contacts.Keys)
            {
                people.Add(new Person(key.FirstName, key.LastName, key.Birthday, Guid.Empty.ToString()));
            }
            return people;
        }
    }
}
