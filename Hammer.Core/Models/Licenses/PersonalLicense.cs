using System;
using System.ComponentModel;

namespace Hammer.Core.Models
{
    /// <summary>
    /// Represents a License for a Person.
    /// </summary>
    public class PersonalLicense : BaseLicense, INotifyPropertyChanged
    {
        private OperatorClasses operatorClass;
        private OperatorClasses previousOperatorClass;

        /// <summary>
        /// Represents a license's class. Only applies to licenses issued to people.
        /// </summary>
        public OperatorClasses OperatorClass
        {
            get => operatorClass;
            set
            {
                operatorClass = value;
                RaisePropertyChanged();
            }
        }

        public OperatorClasses PreviousOperatorClass
        {
            get => previousOperatorClass;
            set
            {
                previousOperatorClass = value;
                RaisePropertyChanged();
            }
        }

        public PersonalLicense()
        {
            LicenseeType = LicenseeTypes.Person;
        }
    }
}
